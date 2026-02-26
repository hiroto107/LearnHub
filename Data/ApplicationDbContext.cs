using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using LearnHub.Models;

namespace LearnHub.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext(options)
{
    public DbSet<Resource> Resources => Set<Resource>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Bookmark> Bookmarks => Set<Bookmark>();
    public DbSet<Comment> Comments => Set<Comment>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Bookmark>()
            .HasIndex(b => new { b.UserId, b.ResourceId })
            .IsUnique();
    }
}
