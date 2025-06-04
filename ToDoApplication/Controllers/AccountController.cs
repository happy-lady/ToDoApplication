using System.Diagnostics;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;
using ToDoApplication.Data;
using ToDoApplication.Models;
using BCrypt;
using Npgsql;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace ToDoApplication.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly ToDoAppDbContext _context;
        private readonly SignInManager<UserDto> _signInManager;
        private readonly UserManager<UserDto> _userManager;

        public AccountController(ILogger<AccountController> logger, ToDoAppDbContext context, SignInManager<UserDto> signInManager, UserManager<UserDto> userManager)
        {
            _logger = logger;
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
        }
        public IActionResult Login()
        {
            return View();
        }

        // Possibly add this when working on the frontend | TODO
        /*[HttpPost]
        public IActionResult Login([FromBody] LoginRequest loginRequest)
        {
            // TODO - Verify this is working next.
            var user = _context.Users.FirstOrDefault(u => u.UserName == loginRequest.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(loginRequest.Password, user.PasswordHash))
                return Unauthorized("Invalid credentials");

            // If authentication is successful, you can return user data or a JWT token
            return Ok(new { message = "Login successful", user = user.UserName });
        }*/

        [HttpPost]
        public async Task<IActionResult> LogInUser(LoginUserViewModel model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, isPersistent: true, lockoutOnFailure: false);

            if (!result.Succeeded)
            {
                return Unauthorized(new { message = "Invalid credentials" });
            }

            return RedirectToAction("Index", "Home");
        }
        public IActionResult UserRegistration()
        {
            return View();
        }
        public async Task<IActionResult> LogOutUser()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }

        [HttpPost]
        public async Task<IActionResult> RegisterUser(RegisterUserViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.Password);

            var user = new UserDto
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.Username,
            };
            _context.Users.Add(user);
            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                    Console.WriteLine(error.Description);
                }
                return BadRequest(ModelState);
            }

            return RedirectToAction("Login");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
