using Microsoft.AspNetCore.Identity;
using ToDoApplication.Data;
using ToDoApplication.Models;
using Microsoft.EntityFrameworkCore;

namespace ToDoApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            

            builder.Services.AddAuthorization();
            builder.Services.AddAuthentication().AddCookie(IdentityConstants.ApplicationScheme);

            builder.Services.AddIdentityCore<User>()
                .AddEntityFrameworkStores<ToDoAppDbContext>()
                .AddApiEndpoints();

            builder.Services.AddDbContext<ToDoAppDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("Database")));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Account}/{action=Login}/{id?}");

            app.Run();
        }
    }
}
