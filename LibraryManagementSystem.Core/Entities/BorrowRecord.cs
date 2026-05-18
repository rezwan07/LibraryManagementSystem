using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Core.Entities
{
    public class BorrowRecord : BaseEntity
    {
        public int UserId { get; set; }

        public int BookId { get; set; } 

        public DateTime BorrowDate { get; set; } = DateTime.UtcNow;

        public DateTime DueDate { get; set; }

        public DateTime? ReturnDate { get; set; }

        public decimal? FineAmount { get; set; }

        public bool IsFinePaid { get; set; } = false;

        public string Status { get; set; } = "Borrowed";

        public virtual User User { get; set; } = null;

        public virtual Book Book { get; set; } = null;
    }
}
