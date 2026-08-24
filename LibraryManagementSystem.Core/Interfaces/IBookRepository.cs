using LibraryManagementSystem.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Core.Interfaces
{
    public interface IBookRepository : IGenericRepository<Book>
    {
        Task<IEnumerable<Book>> GetAvailableBooksAsync();

        Task<IEnumerable<Book>> GetBookAuthorAsync(string author);

        Task<IEnumerable<Book>> GetBookByCategoryAsync(string category);

        Task<Book?> GetBookByISBNAsync(string isbn);

        Task UpdateAvailabilityAsync(int bookId, int changeAmount);


    }
}
