using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Services.DTOs
{
    public class CreateBookDTO
    {
        [Required(ErrorMessage = "ISBN is required")]
        [MaxLength(20)]
        [Display(Name = "ISBN")]
        public string ISBN { get; set; } = string.Empty;

        [Required(ErrorMessage = "Book Name is Required")]
        [MaxLength(200)]
        [Display(Name = "Book Name")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Author Name Required")]
        [MaxLength(100)]
        [Display(Name = "Author Name")]
        public string Author { get; set; } = string.Empty;

        [MaxLength(100)]
        [Display(Name = "Publisher")]
        public string? Publisher { get; set; }

        [Range(1000, 2026, ErrorMessage = "valid in (1000-2026)")]
        [Display(Name = "Publication Year")]
        public int PublicationYear { get; set; }

        [MaxLength(50)]
        [Display(Name = "Category")]
        public string? Category { get; set; }

        [Range(1, 1000, ErrorMessage = "Copies between 1-1000")]
        [Display(Name = "Total Copies")]
        public int TotalCopies { get; set; }

        [MaxLength(100)]
        [Display(Name = "Location")]
        public string? Location { get; set; }
    }
}
