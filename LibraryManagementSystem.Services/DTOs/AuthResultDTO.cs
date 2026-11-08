using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Services.DTOs
{
    public class AuthResultDTO
    {
        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }
        public string? Token { get; set; }
        public UserDTO? User { get; set; }
    }
}
