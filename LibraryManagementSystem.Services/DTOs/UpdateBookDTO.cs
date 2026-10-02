using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Services.DTOs
{
    public class UpdateBookDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "ISBN is required")]
        [MaxLength(20)]
        public string ISBN { get; set; } = string.Empty;

        [Required(ErrorMessage = "Book Name is Required")]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Author Name is Required")]
        [MaxLength(100)]
        public string Author { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? Publisher { get; set; }

        [Range(1000, 2026)]
        public int PublicationYear { get; set; }

        [MaxLength(50)]
        public string? Category { get; set; }

        [Range(1, 1000)]
        public int TotalCopies { get; set; }

        [MaxLength(100)]
        public string? Location { get; set; }
    }
}
