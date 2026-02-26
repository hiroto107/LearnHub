using System.ComponentModel.DataAnnotations;

namespace LearnHub.Models;

public class Bookmark
{
    public int Id { get; set; }

    [Required]
    public string UserId { get; set; } = string.Empty;

    public int ResourceId { get; set; }
    public Resource? Resource { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
