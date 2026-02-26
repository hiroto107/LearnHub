using System.ComponentModel.DataAnnotations;

namespace LearnHub.Models;

public class Comment
{
    public int Id { get; set; }

    [Required]
    public string UserId { get; set; } = string.Empty;

    public int ResourceId { get; set; }
    public Resource? Resource { get; set; }

    [Required]
    [StringLength(500)]
    public string Body { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsHidden { get; set; }
}
