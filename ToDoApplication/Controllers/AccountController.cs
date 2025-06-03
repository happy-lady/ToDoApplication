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
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;

        public AccountController(ILogger<AccountController> logger, ToDoAppDbContext context, SignInManager<User> signInManager, UserManager<User> userManager)
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

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            var result = await _signInManager.PasswordSignInAsync(username, password, isPersistent: true, lockoutOnFailure: false);

            if (!result.Succeeded)
            {
                return Unauthorized(new { message = "Invalid credentials" });
            }
            Console.WriteLine($"Typed user: {username}");
            Console.WriteLine($"User IsAuthenticated: {HttpContext.User.Identity.IsAuthenticated}");

            return RedirectToAction("Index", "Home");
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

        public async Task<IActionResult> LogInUser(string username, string password)
        {
            var test = await Login(username, password);
            return RedirectToAction("Index", "Home");
        }
        public IActionResult UserRegistration()
        {
            return View();
        }
        public IActionResult LogOutUser()
        {
            return RedirectToAction("Login");
        }

        [HttpPost]
        public async Task<IActionResult> RegisterUser(RegisterUserDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.Password);

            var user = new User
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
