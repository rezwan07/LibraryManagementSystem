using LibraryManagementSystem.Core.Exceptions;
using LibraryManagementSystem.Services.DTOs;
using LibraryManagementSystem.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace LibraryManagementSystem.Web.Controllers
{
    public class BooksController : Controller
    {
        private readonly IBookService _bookService;
        private readonly ILogger<BooksController> _logger;
        public BooksController(IBookService bookService, ILogger<BooksController> logger)
        {
            _bookService = bookService;
            _logger = logger;
        }

        public async Task<IActionResult> Index(
            string? searchTerm = null,
            int page = 1,
            int pageSize = 10)
        {
            _logger.LogInformation("Books Index accessed. Search: {SearchTerm}, Page: {Page}", searchTerm, page);

            var books = string.IsNullOrEmpty(searchTerm)
                ? await _bookService.GetAllBooksAsync()
                : await _bookService.SearchBooksAsync(searchTerm);

            var totalItems = books.Count();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var paginatedBooks = books
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewData["SearchTerm"] = searchTerm;
            ViewData["CurrentPage"] = page;
            ViewData["TotalPages"] = totalPages;
            ViewData["PageSize"] = pageSize;
            ViewData["TotalItems"] = totalItems;

            return View(paginatedBooks);

        }

        public async Task<IActionResult> Details(int id)
        {
            _logger.LogInformation("Viewing book details. ID: {BookId}", id);

            var book = await _bookService.GetBookByIdAsync(id);

            if(book == null)
            {
                _logger.LogWarning("Book not found. ID: {BookId}", id);
                return NotFound();
            }

            return View(book);
        }

        public IActionResult Create()
        {
            return View(new CreateBookDTO());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateBookDTO createDto)
        {
            if (!ModelState.IsValid)
            {
                return View(createDto);
            }
            try
            {
                var createdBook = await _bookService.CreateBookAsync(createDto);
                TempData["successMessage"] = $"'{createdBook.Title}' successfully added!";
                return RedirectToAction(nameof(Index));
            }
            catch(DuplicateISBNException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(createDto);
            }

            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(createDto);
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            var book = await _bookService.GetBookByIdAsync(id);

            if(book == null)
            {
                return NotFound();
            }

            var updateDto = new UpdateBookDTO
            {
                Id = book.Id,
                ISBN = book.ISBN,
                Title = book.Title,
                Author = book.Author,
                Publisher = book.Publisher,
                PublicationYear = book.PublicationYear,
                Category = book.Category,
                TotalCopies = book.TotalCopies,
                Location = book.Location

            };

            return View(updateDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdateBookDTO updateDto)
        {
            if(id != updateDto.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(updateDto);
            }

            try
            {
                await _bookService.UpdateBookAsync(updateDto);

                TempData["SuccessMessage"] = "Book updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch(BookNotFoundException ex)
            {
                return NotFound(ex.Message);
            }

            catch(DuplicateISBNException ex)
            {
                ModelState.AddModelError(nameof(updateDto.ISBN), ex.Message);
                return View(updateDto);
            }
            catch(LibraryException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(updateDto);
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            var book = await _bookService.GetBookByIdAsync(id);

            if(book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _bookService.DeleteBookAsync(id);

                TempData["SuccessMessage"] = "Book deleted successfully!";
                return RedirectToAction(nameof(Index));
            }

            catch(BookNotFoundException)
            {
                return NotFound();
            }
            catch(BookNotAvailableException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
