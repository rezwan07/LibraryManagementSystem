using AutoMapper;
using LibraryManagementSystem.Core.Entities;
using LibraryManagementSystem.Core.Enums;
using LibraryManagementSystem.Core.Exceptions;
using LibraryManagementSystem.Core.Interfaces;
using LibraryManagementSystem.Services.DTOs;
using LibraryManagementSystem.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly ILogger<UserService> _logger;

        public UserService(
            IUnitOfWork unitOfWork, 
            IMapper mapper, 
            IJwtTokenService jwtTokenService, 
            ILogger<UserService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _jwtTokenService = jwtTokenService;
            _logger = logger;
        }

        public async Task<AuthResultDTO> RegisterAsync(RegisterDTO registerDtO)
        {
            _logger.LogInformation("Registration attempt for email: {Email}", registerDtO.Email);

            var existingUser = await _unitOfWork.Users.FindAsync(u => u.Email == registerDtO.Email && !u.IsDeleted);

            if (existingUser.Any())
            {
                return new AuthResultDTO
                {
                    Success = false,
                    ErrorMessage = "This email is already registered"
                };
            }

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(registerDtO.Password);

            var user = new User
            {
                FullName = registerDtO.FullName,
                Email = registerDtO.Email,
                PhoneNumber = registerDtO.PhoneNumber,
                PasswordHash = passwordHash,
                Role = UserRole.Member.ToString(),
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                IsDeleted = false
            };

            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.SaveChangeAsync();

            _logger.LogInformation("User registered successfully. Id: {UserId}", user.Id);

            var token = _jwtTokenService.GenerateToken(user);

            return new AuthResultDTO
            {
                Success = true,
                Token = token,
                User = _mapper.Map<UserDTO>(user)
            };
        }

        public async Task<AuthResultDTO> LoginAsync(LoginDTO loginDto)
        {
            _logger.LogInformation("Login attempt for email: {Email}", loginDto.Email);

            var users = await _unitOfWork.Users.FindAsync(
                u => u.Email == loginDto.Email && !u.IsDeleted);

            var user = users.FirstOrDefault();

            if(user == null)
            {
                _logger.LogWarning("Login failed - user not found: {Email}", loginDto.Email);
                return new AuthResultDTO
                {
                    Success = false,
                    ErrorMessage = "Invalid email or password"
                };
            }

            if(!BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
            {
                _logger.LogWarning("Login failed - wrong password: {Email}", loginDto.Email);

                return new AuthResultDTO
                {
                    Success = false,
                    ErrorMessage = "Invalid email or password"
                };
            }

            if (!user.IsActive)
            {
                _logger.LogWarning("Login failed - inactive account: {Email}", loginDto.Email);
                return new AuthResultDTO
                {
                    Success = false,
                    ErrorMessage = "Your account is disabled. Contact admin."
                };
            }

            var token = _jwtTokenService.GenerateToken(user);

            _logger.LogInformation("Login successful. User: {UserId}", user.Id);

            return new AuthResultDTO
            {
                Success = true,
                Token = token,
                User = _mapper.Map<UserDTO>(user)
            };
        }

        public async Task<IEnumerable<UserDTO>> GetAllUsersAsync()
        {
            var users = await _unitOfWork.Users.GetAllAsync();

            return _mapper.Map<IEnumerable<UserDTO>>(users);
        }

        public async Task<UserDTO?> GetUserByIdAsync(int id)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(id);
            return user == null ? null : _mapper.Map<UserDTO>(user);
        }

        public async Task<UserDTO?> GetUserByEmailAsync(string email)
        {
            var users = await _unitOfWork.Users.FindAsync(
                u => u.Email == email && !u.IsDeleted);

            var user = users.FirstOrDefault();
            return user == null ? null : _mapper.Map<UserDTO>(user);
        }

        public async Task<bool> ToggleUserStatusAsync(int id)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(id);

            if(user == null)
            {
                throw new UserNotFoundException($"User with ID {id} not found");
            }

            user.IsActive = !user.IsActive;
            user.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.Users.UpdateAsync(user);

            _logger.LogInformation(
                "User status toggled. Id: {UserId}, Active: {IsActive}", id, user.IsActive);

            return true;
        }
    }
}
