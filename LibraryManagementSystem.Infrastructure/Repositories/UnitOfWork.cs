using LibraryManagementSystem.Core.Entities;
using LibraryManagementSystem.Core.Interfaces;
using LibraryManagementSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly LibraryDbContext _context;
        private IDbContextTransaction? _transaction;
        private bool _disposed = false;

        private IGenericRepository<User>? _userRepository;
        private IBookRepository? _bookRepository;
        private IGenericRepository<BorrowRecord>? _borrowRecordRepository;

        public UnitOfWork(LibraryDbContext context)
        {
            _context = context;
        } 

        public IGenericRepository<User> Users
        {
            get
            {
                if (_userRepository == null)
                    _userRepository = new GenericRepository<User>(_context);
                return _userRepository;
            }
        }

        public IBookRepository Books
        {
            get
            {
                if(_bookRepository == null)
                    _bookRepository = new BookRepository(_context);
                return _bookRepository;
            }
        }

        public IGenericRepository<BorrowRecord> BorrowRecords
        {
            get
            {
                if (_borrowRecordRepository == null)
                    _borrowRecordRepository = new GenericRepository<BorrowRecord>(_context);
                return _borrowRecordRepository;
            }
        }

        public async Task<int> SaveChangeAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task BeginTransactionAsync()
        {
            _transaction= await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransaction()
        {
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if(_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction= null;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    {
                        _transaction?.Dispose();
                        _context.Dispose();
                    }
                    _disposed = true;
                }
            }
        }

    }


}
