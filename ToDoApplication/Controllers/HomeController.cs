using System.Diagnostics;
using System.Threading.Tasks;
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

        public IActionResult CreateEditToDoItem()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateEditToDoItemForm(ToDoItem item)
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
        //[HttpDelete]

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
