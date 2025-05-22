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
        public async Task<IActionResult> Login(string username, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserName == username);
            if (username == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                // return Unauthorized("Invalid credentials");
                return Unauthorized(new { message = "Invalid credentials" });
            }
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties { IsPersistent = true };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity), authProperties);

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
            // Console.WriteLine($"Database Provider: {_context.Database.ProviderName}");
            // Console.WriteLine($"New User: {user}");
            _context.Users.Add(user);
            var result = await _context.SaveChangesAsync();

            // Console.WriteLine($"SaveChangesAsync() result: {result}");

            if (result == 0)
            {
                Console.WriteLine("User was not saved.");
                return BadRequest("User was not saved.");
            }

            // Console.WriteLine($"User {user.UserName} registered successfully.");
            return RedirectToAction("Login");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
