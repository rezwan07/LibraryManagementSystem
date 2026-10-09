using LibraryManagementSystem.Infrastructure;
using LibraryManagementSystem.Infrastructure.Data;
using LibraryManagementSystem.Services;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllersWithViews();
            builder.Services.AddInfrastructure(builder.Configuration);
            builder.Services.AddApplicationServices();

            var app = builder.Build(); 

            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<LibraryDbContext>();
                try
                {
                    await dbContext.Database.MigrateAsync();
                    Console.WriteLine("Database migrated successfully");
                }

                catch(Exception ex)
                {
                    Console.WriteLine($"Migration failed: {ex.Message}");
                    throw;
                }
            }
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            await app.RunAsync();
        }
    }
}