using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Services.DTOs
{
    public class BookDTO
    {
        public int Id {  get; set; }
        public string ISBN { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string? Publisher { get; set; }
        public int PublicationYear { get; set; }
        public string? Category { get; set; }
        public int TotalCopies { get; set; }
        public int AvailableCopies { get; set; }
        public string? Location { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsAvailable => AvailableCopies > 0;
        public int BorrowedCopies => TotalCopies - AvailableCopies;

    }
}
