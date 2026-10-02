using LibraryManagementSystem.Services.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Services.Interfaces
{
    public interface IBookService
    {
        Task<IEnumerable<BookDTO>> GetAllBooksAsync();
        Task<BookDTO?> GetBookByIdAsync(int id);
        Task<IEnumerable<BookDTO>> GetAvailableBooksAsync();
        Task<IEnumerable<BookDTO>> SearchBooksAsync(string searchTerm);
        Task<BookDTO> CreateBookAsync(CreateBookDTO createDto);
        Task<BookDTO> UpdateBookAsync(UpdateBookDTO updateDto);
        Task<bool> DeleteBookAsync(int id);
        Task<bool> IsBookAvailableAsync(int bookId);
        Task<int> GetTotalCopiesAsync(int bookId);
        Task<int> GetBorrowedCopiesCountAsync(int bookId);


    }
}
