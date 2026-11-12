using LibraryManagementSystem.Services.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Services.Interfaces
{
    public interface IUserService
    {
        Task<AuthResultDTO> RegisterAsync(RegisterDTO registerDtO);
        Task<AuthResultDTO> LoginAsync(LoginDTO loginDto);
        Task<IEnumerable<UserDTO>> GetAllUsersAsync();
        Task<UserDTO?> GetUserByIdAsync(int id);
        Task<UserDTO?> GetUserByEmailAsync(string email);
        Task<bool> ToggleUserStatusAsync(int id);

    }
}
