using LearnHub.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LearnHub.Controllers;

[Authorize]
public class MemberController(
    ApplicationDbContext context,
    UserManager<IdentityUser> userManager) : Controller
{
    public async Task<IActionResult> Index()
    {
        var userId = userManager.GetUserId(User);
        if (userId is null)
        {
            return Challenge();
        }

        var bookmarkCount = await context.Bookmarks
            .FromSqlInterpolated($"SELECT * FROM Bookmarks WHERE UserId = {userId}")
            .CountAsync();
        ViewData["BookmarkCount"] = bookmarkCount;
        return View();
    }

    public async Task<IActionResult> Bookmarks()
    {
        var userId = userManager.GetUserId(User);
        if (userId is null)
        {
            return Challenge();
        }

        var bookmarks = await context.Bookmarks
            .FromSqlInterpolated($"SELECT * FROM Bookmarks WHERE UserId = {userId}")
            .Include(b => b.Resource)
            .ThenInclude(r => r!.Category)
            .OrderByDescending(b => b.CreatedAt)
            .ToListAsync();

        return View(bookmarks);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddBookmark(int resourceId)
    {
        var userId = userManager.GetUserId(User);
        if (userId is null)
        {
            return Challenge();
        }

        var resourceExists = await context.Resources.AnyAsync(r => r.Id == resourceId);
        if (!resourceExists)
        {
            return NotFound();
        }

        var alreadyAdded = await context.Bookmarks
            .FromSqlInterpolated($"SELECT * FROM Bookmarks WHERE UserId = {userId} AND ResourceId = {resourceId}")
            .AnyAsync();
        if (!alreadyAdded)
        {
            await context.Database.ExecuteSqlInterpolatedAsync($@"
                INSERT INTO Bookmarks (UserId, ResourceId, CreatedAt)
                VALUES ({userId}, {resourceId}, {DateTime.UtcNow})");
        }

        return RedirectToAction(nameof(Bookmarks));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RemoveBookmark(int id)
    {
        var userId = userManager.GetUserId(User);
        if (userId is null)
        {
            return Challenge();
        }

        var affected = await context.Database.ExecuteSqlInterpolatedAsync(
            $"DELETE FROM Bookmarks WHERE Id = {id} AND UserId = {userId}");
        if (affected == 0)
        {
            return NotFound();
        }
        return RedirectToAction(nameof(Bookmarks));
    }
}
