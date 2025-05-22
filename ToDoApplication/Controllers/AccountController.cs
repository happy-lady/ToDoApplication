using System.Diagnostics;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;
using ToDoApplication.Data;
using ToDoApplication.Models;
using BCrypt;
using Npgsql;

namespace ToDoApplication.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly ToDoAppDbContext _context;

        public AccountController(ILogger<AccountController> logger, ToDoAppDbContext context)
        {
            _logger = logger;
            _context = context;
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public User Login(string username, string password)
        {
            if (username != null || password != null)
            {
                Login();
            }
            else
            {
                return _context.Users
                .FirstOrDefault(u => u.UserName == username && u.PasswordHash == password);
            }
            return null;
        }

        [HttpPost]
        public IActionResult Login([FromBody] LoginRequest loginRequest)
        {
            // TODO - Verify this is working next.
            var user = _context.Users.FirstOrDefault(u => u.UserName == loginRequest.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(loginRequest.Password, user.PasswordHash))
                return Unauthorized("Invalid credentials");

            // If authentication is successful, you can return user data or a JWT token
            return Ok(new { message = "Login successful", user = user.UserName });
        }

        public IActionResult LogInUser(string username, string password)
        {
            var test = Login(username, password);
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
                PasswordHash = hashedPassword
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return RedirectToAction("Login");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
