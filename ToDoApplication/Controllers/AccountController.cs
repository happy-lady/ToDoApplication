using Microsoft.AspNetCore.Mvc;

namespace ToDoApplication.Controllers
{
    public class AccountController : Controller
    {
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
    }
}
