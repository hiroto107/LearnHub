using LearnHub.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LearnHub.Controllers;

public class ResourcesController(ApplicationDbContext context) : Controller
{
    public async Task<IActionResult> Index(string? q)
    {
        var query = context.Resources
            .FromSqlRaw("SELECT * FROM Resources")
            .Include(r => r.Category)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(q))
        {
            query = query.Where(r => r.Title.Contains(q) || r.Description.Contains(q));
        }

        var resources = await query
            .OrderByDescending(r => r.UpdatedAt)
            .ToListAsync();

        ViewData["Query"] = q;
        return View(resources);
    }

    public async Task<IActionResult> Details(int id)
    {
        var resource = await context.Resources
            .FromSqlInterpolated($"SELECT * FROM Resources WHERE Id = {id}")
            .Include(r => r.Category)
            .FirstOrDefaultAsync();

        if (resource is null)
        {
            return NotFound();
        }

        return View(resource);
    }
}
