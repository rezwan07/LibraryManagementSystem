using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Core.Entities
{
    public class User : BaseEntity
    {
        [Required(ErrorMessage = "full name is required")]
        [MaxLength(100, ErrorMessage = "maximum 100 letters allowed")]
        [Display(Name = "Full Name", Description = "Enter your full name")]
        public string FullName { get; set; } = string.Empty;

        [Required (ErrorMessage = " email is required")]
        [EmailAddress (ErrorMessage ="invalid email format")]
        [Display (Name = "Email", Description = "Enter your email address")]
        public String Email { get; set; }

        [Required]
        public String PasswordHash { get; set; } = string.Empty;

        [Required]
        public string Role { get; set; } = "member";

        [MaxLength(20)]
        [Phone(ErrorMessage = "invalid phone number format")]
        [Display(Name = "Phone Number")]
        public string? PhoneNumber { get; set; }

        public bool IsActive { get; set; } = true;

        public virtual ICollection<BorrowRecord> BorrowRecords { get; set; } = new List<BorrowRecord>();

    }
}
