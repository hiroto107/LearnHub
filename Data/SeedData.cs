using LearnHub.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LearnHub.Data;

public static class SeedData
{
    public static async Task InitializeAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

        await db.Database.MigrateAsync();

        foreach (var role in new[] { "Admin", "Member" })
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        const string adminEmail = "admin@learnhub.local";
        const string adminPassword = "Admin123!";

        var adminUser = await userManager.FindByEmailAsync(adminEmail);
        if (adminUser is null)
        {
            adminUser = new IdentityUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(adminUser, adminPassword);
            if (result.Succeeded)
            {
                await userManager.AddToRolesAsync(adminUser, new[] { "Admin", "Member" });
            }
        }

        if (!db.Categories.Any())
        {
            db.Categories.AddRange(
                new Category { Name = "Programming" },
                new Category { Name = "Math" },
                new Category { Name = "Science" }
            );
            await db.SaveChangesAsync();
        }

        if (!db.Resources.Any())
        {
            var programming = await db.Categories.FirstAsync(c => c.Name == "Programming");
            var science = await db.Categories.FirstAsync(c => c.Name == "Science");

            db.Resources.AddRange(
                new Resource
                {
                    Title = "ASP.NET Core MVC Fundamentals",
                    Description = "Official Microsoft learning path for ASP.NET Core MVC basics.",
                    Url = "https://learn.microsoft.com/aspnet/core/mvc/overview",
                    MediaType = "Article",
                    Level = 2,
                    CategoryId = programming.Id
                },
                new Resource
                {
                    Title = "MIT OpenCourseWare - Physics",
                    Description = "Free lecture videos and resources for introductory physics.",
                    Url = "https://ocw.mit.edu/courses/physics/",
                    MediaType = "Video",
                    Level = 3,
                    CategoryId = science.Id
                }
            );
            await db.SaveChangesAsync();
        }
    }
}
