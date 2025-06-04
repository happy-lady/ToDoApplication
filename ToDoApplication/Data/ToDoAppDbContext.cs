using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ToDoApplication.Models;

namespace ToDoApplication.Data
{
    public class ToDoAppDbContext : IdentityDbContext<UserDto>
    {
        /*DbSet<UserDto> Users { get; set; }
        DbSet<ToDoItem> ToDoItems { get; set; }*/
        public ToDoAppDbContext(DbContextOptions<ToDoAppDbContext> options)
            : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<UserDto>().Property(u => u.FirstName).HasMaxLength(10);
            builder.Entity<UserDto>().Property(u => u.LastName).HasMaxLength(10);
        }
    }
}
