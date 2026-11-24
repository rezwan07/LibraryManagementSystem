using LibraryManagementSystem.Services.DTOs;
using LibraryManagementSystem.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace LibraryManagementSystem.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        private readonly ILogger<AccountController> _logger;
        public AccountController( IUserService userService, ILogger<AccountController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Register()
        {
            return View(new RegisterDTO());
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterDTO registerDto)
        {
            if (!ModelState.IsValid)
                return View(registerDto);

            var result = await _userService.RegisterAsync(registerDto);

            if (!result.Success)
            {
                ModelState.AddModelError("", result.ErrorMessage ?? "Registration faield");
                return View(registerDto);
            }
               
            await SignInUserAsync(result);

            TempData["SuccessMessage"] = "Registration successful! Welcome!" ;
            return RedirectToAction("Index","Home");

        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View(new LoginDTO());
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginDTO loginDto, string? returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                ViewData["ReturnUrl"] = returnUrl;
                return View(loginDto);
            }

            var result = await _userService.LoginAsync(loginDto);

            if (!result.Success)
            {
                ModelState.AddModelError("", result.ErrorMessage ?? "Login failed");
                ViewData["ReturnUrl"] = returnUrl;
                return View(loginDto);
            }

            await SignInUserAsync(result, loginDto.RememberMe);
            TempData["SuccessMessage"] = $"Welcome back, {result.User!.FullName}!";

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            return RedirectToAction("Index", "Home");

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            Response.Cookies.Delete("access_token", new CookieOptions
            {
                Path = "/",
                Secure = true,
                HttpOnly = true,
                SameSite = SameSiteMode.Strict
            });

            TempData["SuccessMessage"] = "Logged out successfully";

            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        private async Task SignInUserAsync(AuthResultDTO result, bool rememberMe = false)
        {
            if (string.IsNullOrEmpty(result.Token))
                return;

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = rememberMe
                    ? DateTimeOffset.UtcNow.AddDays(7)
                    : DateTimeOffset.UtcNow.AddHours(1)
            };

            Response.Cookies.Append("access_token", result.Token, cookieOptions);
            _logger.LogInformation("User signed in. Email: {Email}", result.User?.Email);
        }
    }
}
