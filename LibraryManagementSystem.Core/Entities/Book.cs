using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Core.Entities
{
    public class Book : BaseEntity
    {
        [Required(ErrorMessage = "ISBN is required")]
        [MaxLength(20)]
        [Display(Name = "ISBN")]
        public string ISBN { get; set; } = string.Empty;


        [Required(ErrorMessage = "Title is required")]
        [MaxLength(200)]
        [Display(Name = "Book Name")]
        public string Title { get; set; } = string.Empty;


        [Required(ErrorMessage = "author name is required")]
        [MaxLength(100)]
        [Display(Name = "Author")]
        public string Author { get; set; } = string.Empty;

        [MaxLength(100)]
        [Display(Name = "Publisher")]
        public string? Publisher { get; set; }

        [Range(1000,2026, ErrorMessage = "valid in (1000-2026)")]
        [Display (Name = "Publication Year")]
        public int PublicationYear { get; set; }

        [MaxLength(50)]
        [Display(Name = "Category")]
        public string? Category { get; set; }


        public int TotalCopies { get; set; }

        [Display (Name ="Available Copies")]
        public int AvailableCopies { get; set; }

        [MaxLength(100)]
        [Display(Name = "Location")]
        public string? Location { get; set; }


        public virtual ICollection<BorrowRecord> BorrowRecords { get; set; } = new List<BorrowRecord>();

    }
}
