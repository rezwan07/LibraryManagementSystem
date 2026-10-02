using AutoMapper;
using LibraryManagementSystem.Core.Entities;
using LibraryManagementSystem.Core.Interfaces;
using LibraryManagementSystem.Services.DTOs;
using LibraryManagementSystem.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Services.Implementations
{
    public class BookService : IBookService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<BookService> _logger;

        public BookService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<BookService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<BookDTO>> GetAllBooksAsync()
        {
            _logger.LogInformation("Getting all books");
            var books = await _unitOfWork.Books.GetAllAsync();

            return _mapper.Map<IEnumerable<BookDTO>>(books);
        }

        public async Task<BookDTO?> GetBookByIdAsync(int id)
        {
            _logger.LogInformation("Getting book with ID: {BookId}", id);
            var book = await _unitOfWork.Books.GetByIdAsync(id);

            if (book == null)
            {
                _logger.LogWarning("Book not found with ID: {BookId}", id);
                return null;
            }

            return _mapper.Map<BookDTO>(book);
        }

        public async Task<IEnumerable<BookDTO>> GetAvailableBooksAsync()
        {
            var books = await _unitOfWork.Books.GetAvailableBooksAsync();
            return _mapper.Map<IEnumerable<BookDTO>>(books);
        }

        public async Task<IEnumerable<BookDTO>> SearchBooksAsync(string searchTerm)
        {
            _logger.LogInformation("Searching books with term: {Term}", searchTerm);

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return await GetAllBooksAsync();
            }

            var books = await _unitOfWork.Books.FindAsync(b => !b.IsDeleted && (

                                                                b.Title.Contains(searchTerm) ||
                                                                b.Author.Contains(searchTerm) ||
                                                                b.ISBN.Contains(searchTerm)
                                                         ));

            return _mapper.Map<IEnumerable<BookDTO>>(books);
        }

        public async Task<BookDTO> CreateBookAsync(CreateBookDTO createDto)
        {
            _logger.LogInformation("Creating new book: {Title}", createDto.Title);

            var existingBook = await _unitOfWork.Books.GetBookByISBNAsync(createDto.ISBN);

            if (existingBook != null)
            {
                throw new DuplicateISBNException(
                    $"Book with ISBN '{createDto.ISBN}' already exist ");
            }

            var book = _mapper.Map<Book>(createDto);

            book.AvailableCopies = book.TotalCopies;

            book.CreatedAt = DateTime.UtcNow;
            book.IsDeleted = false;

            var createdBook = await _unitOfWork.Books.AddAsync(book);
            _logger.LogInformation("Book created successfully with ID: {BookId}", createdBook.Id);

            return _mapper.Map<BookDTO>(createdBook);
        }

        public async Task<BookDTO> UpdateBookAsync(UpdateBookDTO updateDto)
        {
            _logger.LogInformation("Updating book ID: {BookId}", updateDto.Id);

            var existingBook = await _unitOfWork.Books.GetByIdAsync(updateDto.Id);

            if (existingBook == null)
            {
                throw new BookNotFoundException($"Book with ID {updateDto.Id} not found");
            }

            var bookWithSameISBN = await _unitOfWork.Books.GetBookByISBNAsync(updateDto.ISBN);
            if (bookWithSameISBN != null && bookWithSameISBN.Id != updateDto.Id)
            {
                throw new DuplicateISBNException(
                    $"Another book with ISBN '{updateDto.ISBN}' already exists");
            }

            var copiesDifference = updateDto.TotalCopies - existingBook.TotalCopies;
            var newAvailableCopies = existingBook.AvailableCopies + copiesDifference;

            _mapper.Map(updateDto, existingBook);

            existingBook.AvailableCopies = Math.Max(0, newAvailableCopies);

            existingBook.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.Books.UpdateAsync(existingBook);


            _logger.LogInformation("Book updated successfully: {BookId}", existingBook.Id);

            return _mapper.Map<BookDTO>(existingBook);

        }
        public async Task<bool> DeleteBookAsync(int id)
        {
            _logger.LogInformation("Deleting book ID: {BookId}", id);
            var book = await _unitOfWork.Books.GetByIdAsync(id);

            if(book == null)
            {
                throw new BookNotFoundException($"Book with ID {id} not found")
            }

            var borrowedCount = book.TotalCopies - book.AvailableCopies;

            if(borrowedCount > 0)
            {
                throw new BookNotAvailableException(
                    $"Cannot delete book. {borrowedCount} copies are currently borrowed");
            }

            await _unitOfWork.Books.DeleteAsync(id);
            _logger.LogInformation("Book deleted successfully: {BookId}", id);

            return true;
        }

        public async Task<bool> IsBookAvailableAsync(int bookId)
        {
            var book = await _unitOfWork.Books.GetByIdAsync(bookId);
            return book != null && book.AvailableCopies > 0;
        }

        public async Task<int> GetTotalCopiesAsync(int bookId)
        {
            var book = await _unitOfWork.Books.GetByIdAsync(bookId);
            return book?.TotalCopies ?? 0;
        }

        public async Task<int> GetBorrowedCopiesCountAsync(int bookId)
        {
            var book = await _unitOfWork.Books.GetByIdAsync(bookId);
            if (book == null) return 0;

            return book.TotalCopies - book.AvailableCopies;
        }

    }
}
