using LibraryManagementSystem.Core.Exceptions;
using LibraryManagementSystem.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;

namespace LibraryManagementSystem.Web.Controllers
{
    [Authorize(Roles ="Admin")]
    public class UsersController : Controller
    {
        private readonly IUserService _userService;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IUserService userService,
            ILogger<UsersController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _userService.GetAllUsersAsync();
            return View(users);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            try
            {
                var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (currentUserId == id.ToString())
                {
                    TempData["ErrorMessage"] = "You cannot deactivate your own account.";
                    return RedirectToAction(nameof(Index));
                }

                await _userService.ToggleUserStatusAsync(id);
                TempData["SuccessMessage"] = "User status updated.";
            }
            catch (UserNotFoundException)
            {
                TempData["ErrorMessage"] = "User not found.";
            }

            return RedirectToAction(nameof(Index));
        }

    }
}
