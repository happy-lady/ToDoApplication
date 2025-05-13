using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ToDoApplication.Data;
using ToDoApplication.Models;

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
        public IActionResult UserRegistration()
        {
            return View();
        }
        public IActionResult LogInUser()
        {
            return RedirectToAction("Index", "Home");
        }
        public IActionResult LogOutUser()
        {
            return RedirectToAction("Login");
        }
        public IActionResult RegisterUser()
        {
            return RedirectToAction("Login");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
