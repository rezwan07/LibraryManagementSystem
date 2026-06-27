using LibraryManagementSystem.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<User> Users { get; }
        IBookRepository Books { get; }
        IGenericRepository<BorrowRecord> BorrowRecords { get; }

        Task<int> SaveChangeAsync();
        Task BeginTransactionAsync();
        Task CommitTransaction();
        Task RollbackTransactionAsync();


    }
}
