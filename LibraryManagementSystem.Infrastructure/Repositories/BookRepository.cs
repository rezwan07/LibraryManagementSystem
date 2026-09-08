using LibraryManagementSystem.Core.Entities;
using LibraryManagementSystem.Core.Interfaces;
using LibraryManagementSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Infrastructure.Repositories
{
    public class BookRepository : GenericRepository<Book>, IBookRepository
    {
        public BookRepository(LibraryDbContext context) : base(context)
        {
                
        }

        public async Task<IEnumerable<Book>> GetAvailableBooksAsync()
        {
            return await _dbSet
                .Where(b => b.AvailableCopies > 0 && !b.IsDeleted)
                .OrderBy(b => b.Title)
                .ToListAsync();
        }

        public async Task<IEnumerable<Book>> GetBookAuthorAsync(string author)
        {
            return await _dbSet
                .Where (b => b.Author.Contains(author) && !b.IsDeleted)
                .OrderBy (b => b.Title)
                .ToListAsync ();
        }

        public async Task<IEnumerable<Book>> GetBookByCategoryAsync(string category)
        {
            return await _dbSet
                .Where(b => b.Category != null && b.Category.Contains(category) && !b.IsDeleted)
                .OrderBy(b => b.Title)
                .ToListAsync();  
        }

        public async Task<Book?> GetBookByISBNAsync(string isbn)
        {
            return await _dbSet
                .FirstOrDefaultAsync(b => b.ISBN == isbn && !b.IsDeleted);
        }

        public async Task UpdateAvailabilityAsync(int bookId, int changeAmount)
        {
            var book = await GetByIdAsync(bookId);
            if (book != null)
            {
                book.AvailableCopies += changeAmount;

                if(book.AvailableCopies < 0)
                    book.AvailableCopies = 0;

                if(book.AvailableCopies < book.TotalCopies)
                    book.AvailableCopies = book.TotalCopies;

                await UpdateAsync(book);
            }
        }
    }
}
