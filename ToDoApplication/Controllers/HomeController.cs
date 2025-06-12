using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoApplication.Data;
using ToDoApplication.Models;

namespace ToDoApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ToDoAppDbContext _context;
        private readonly UserManager<UserDto> _userManager;

        public HomeController(ILogger<HomeController> logger, ToDoAppDbContext context, UserManager<UserDto> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            var toDoItems = await _context.ToDoItems.Where(t => t.UserId == userId).ToListAsync();
            return View(toDoItems);
        }

        public IActionResult CreateToDoItem()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateToDoItemForm(ToDoItem item)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }
            item.UserId = user.Id;
            item.IsCompleted = false;
            _context.ToDoItems.Add(item);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteToDoItem(int id)
        {
            var item = await _context.ToDoItems.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            _context.ToDoItems.Remove(item);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateToDoItemForm(int id, string description)
        {
            var item = await _context.ToDoItems.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            item.Description = description;
            _context.ToDoItems.Update(item);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> UpdateToDoItem(int id)
        {
            var item = await _context.ToDoItems.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateTaskStatus(int id)
        {
            var item = await _context.ToDoItems.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            item.IsCompleted = !item.IsCompleted;
            _context.Update(item);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
