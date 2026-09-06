using LibraryManagementSystem.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Infrastructure.Data
{
    public class LibraryDbContext : DbContext
    {
        public LibraryDbContext(DbContextOptions<LibraryDbContext> options) : base(options)
        {
                
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<BorrowRecord> BorrowRecords { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<BorrowRecord>()
                .HasOne(br => br.User)
                .WithMany(u => u.BorrowRecords)
                .HasForeignKey(br => br.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<BorrowRecord>()
                .HasOne(br => br.Book)
                .WithMany(b => b.BorrowRecords)
                .HasForeignKey(br => br.BookId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique()
                .HasDatabaseName("IX_Users_Email_Unique");

            modelBuilder.Entity<Book>()
                .HasIndex(b => b.ISBN)
                .IsUnique()
                .HasDatabaseName("IX_Books_ISBN_Unique");

            //INDEXES (Performance optimization)
            // Book search optimization
            modelBuilder.Entity<Book>()
                .HasIndex(b => b.Title)
                .HasDatabaseName("IX_Books_Title");

            modelBuilder.Entity<Book>()
                .HasIndex(b => b.Author)
                .HasDatabaseName("IX_Books_Author");

            modelBuilder.Entity<Book>()
                .HasIndex(b => b.Category)
                .HasDatabaseName("IX_Books_Category");

            // BorrowRecord search optimization
            modelBuilder.Entity<BorrowRecord>()
                .HasIndex(br => br.UserId)
                .HasDatabaseName("IX_BorrowRecord_UserId");

            modelBuilder.Entity<BorrowRecord>()
                .HasIndex(br => br.BookId)
                .HasDatabaseName("IX_BorrowRecord_BookId");

            modelBuilder.Entity<BorrowRecord>()
                .HasIndex(br => br.Status)
                .HasDatabaseName("IX_BorrowRecord_Status");

            modelBuilder.Entity<BorrowRecord>()
                .HasIndex(br => br.DueDate)
                .HasDatabaseName("IX_BorrowRecord_DueDate");

            //DEFAULT VALUES

            modelBuilder.Entity<Book>()
                .Property(b => b.CreatedAt)
                .HasDefaultValueSql("GETUTCTIME");

            modelBuilder.Entity<User>()
                .Property(u => u.CreatedAt)
                .HasDefaultValueSql("GETUTCTIME");

            modelBuilder.Entity<BorrowRecord>()
                .Property(br => br.CreatedAt)
                .HasDefaultValueSql("GETUTCTIME");

            modelBuilder.Entity<User>().HasData( new User     
                {
                    Id=1,
                    FullName = "System Administrator",
                    Email = "admin@library.com",
                    PasswordHash = "$2a$11$K8Q8xQ8xQ8xQ8xQ8xQ8xQu",
                    Role = "Admin",
                    PhoneNumber = "01740228229",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    IsDeleted = false

            });

            modelBuilder.Entity<Book>().HasData(
                new Book
                {
                    Id = 1,
                    ISBN = "978-0-452-28423-4",
                    Title = "The Great Gatsby",
                    Author = "F. Scott Fitzgerald",
                    Publisher = "Scribner",
                    PublicationYear = 1925,
                    Category = "Fiction",
                    TotalCopies = 5,
                    AvailableCopies = 5,
                    Location = "Shelf A-1",
                    CreatedAt = DateTime.UtcNow,
                    IsDeleted = false
                },

                new Book
                {
                    Id = 2,
                    ISBN = "978-0-06-112008-4",
                    Title = "To Kill a Mockingbird",
                    Author = "Harper Lee",
                    Publisher = "HarperCollins",
                    PublicationYear = 1960,
                    Category = "Fiction",
                    TotalCopies = 3,
                    AvailableCopies = 3,
                    Location = "Shelf A-2",
                    CreatedAt = DateTime.UtcNow,
                    IsDeleted = false
                },

                new Book
                {
                    Id = 3,
                    ISBN = "978-0-14-143951-8",
                    Title = "1984",
                    Author = "George Orwell",
                    Publisher = "Penguin Books",
                    PublicationYear = 1949,
                    Category = "Science Fiction",
                    TotalCopies = 4,
                    AvailableCopies = 4,
                    Location = "Shelf B-1",
                    CreatedAt = DateTime.UtcNow,
                    IsDeleted = false
                }
            );

        }

    }
}
