using Microsoft.EntityFrameworkCore;
using ToDoApplication.Models;

namespace ToDoApplication.Data
{
    public class ToDoAppDbContext : DbContext
    {
        DbSet<User> Users { get; set; }
        DbSet<ToDoItem> ToDoItems { get; set; }
        public ToDoAppDbContext(DbContextOptions<ToDoAppDbContext> options)
            : base(options)
        {
            
        }
    }
}
