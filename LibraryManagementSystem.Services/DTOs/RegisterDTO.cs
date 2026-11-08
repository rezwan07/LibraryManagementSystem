using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Services.DTOs
{
    public class RegisterDTO
    {
        [Required(ErrorMessage = "Required Full Name")]
        [MaxLength(100)]
        [Display(Name = "Full Name")]
        public string FullName { get; set;} = string.Empty;

        [Required(ErrorMessage = "Required Email")]
        [EmailAddress(ErrorMessage ="give correct email")]
        [MaxLength(150)]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Required Phone Number")]
        [Phone(ErrorMessage = "Give The Correct Number")]
        [MaxLength(20)]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessage ="Password Required")]
        [MinLength(6, ErrorMessage ="minimum 6 digits")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage ="Confirm Password")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage ="Password Not Match")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
