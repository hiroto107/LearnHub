using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using LearnHub.Models;
using LearnHub.Data;
using Microsoft.EntityFrameworkCore;

namespace LearnHub.Controllers;

public class HomeController(ApplicationDbContext context) : Controller
{
    public async Task<IActionResult> Index()
    {
        var latestResources = await context.Resources
            .Include(r => r.Category)
            .OrderByDescending(r => r.UpdatedAt)
            .Take(3)
            .ToListAsync();
        return View(latestResources);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
