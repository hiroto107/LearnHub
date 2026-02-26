using LearnHub.Data;
using LearnHub.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace LearnHub.Controllers;

[Authorize(Roles = "Admin")]
public class AdminResourcesController(ApplicationDbContext context) : Controller
{
    public async Task<IActionResult> Index()
    {
        var resources = await context.Resources
            .FromSqlRaw("SELECT * FROM Resources")
            .Include(r => r.Category)
            .OrderByDescending(r => r.UpdatedAt)
            .ToListAsync();
        return View(resources);
    }

    public async Task<IActionResult> Details(int id)
    {
        var resource = await context.Resources
            .FromSqlInterpolated($"SELECT * FROM Resources WHERE Id = {id}")
            .Include(r => r.Category)
            .FirstOrDefaultAsync();
        return resource is null ? NotFound() : View(resource);
    }

    public IActionResult Create()
    {
        PopulateCategories();
        return View(new Resource());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Resource resource)
    {
        if (!ModelState.IsValid)
        {
            PopulateCategories(resource.CategoryId);
            return View(resource);
        }

        var now = DateTime.UtcNow;
        await context.Database.ExecuteSqlInterpolatedAsync($@"
            INSERT INTO Resources
            (Title, Description, Url, MediaType, Level, CategoryId, CreatedAt, UpdatedAt)
            VALUES
            ({resource.Title}, {resource.Description}, {resource.Url}, {resource.MediaType}, {resource.Level}, {resource.CategoryId}, {now}, {now})");
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var resource = await context.Resources.FindAsync(id);
        if (resource is null)
        {
            return NotFound();
        }

        PopulateCategories(resource.CategoryId);
        return View(resource);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Resource resource)
    {
        if (id != resource.Id)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            PopulateCategories(resource.CategoryId);
            return View(resource);
        }

        var affected = await context.Database.ExecuteSqlInterpolatedAsync($@"
            UPDATE Resources
            SET Title = {resource.Title},
                Description = {resource.Description},
                Url = {resource.Url},
                MediaType = {resource.MediaType},
                Level = {resource.Level},
                CategoryId = {resource.CategoryId},
                UpdatedAt = {DateTime.UtcNow}
            WHERE Id = {id}");

        if (affected == 0)
        {
            return NotFound();
        }

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var resource = await context.Resources
            .FromSqlInterpolated($"SELECT * FROM Resources WHERE Id = {id}")
            .Include(r => r.Category)
            .FirstOrDefaultAsync();
        return resource is null ? NotFound() : View(resource);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var affected = await context.Database.ExecuteSqlInterpolatedAsync(
            $"DELETE FROM Resources WHERE Id = {id}");

        if (affected == 0)
        {
            return NotFound();
        }
        return RedirectToAction(nameof(Index));
    }

    private void PopulateCategories(int selectedId = 0)
    {
        ViewData["CategoryId"] = new SelectList(context.Categories.OrderBy(c => c.Name), "Id", "Name", selectedId);
    }
}
