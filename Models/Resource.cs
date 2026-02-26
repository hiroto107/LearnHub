using System.ComponentModel.DataAnnotations;

namespace LearnHub.Models;

public class Resource
{
    public int Id { get; set; }

    [Required]
    [StringLength(120)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [StringLength(1000)]
    public string Description { get; set; } = string.Empty;

    [Required]
    [Url]
    [Display(Name = "Resource URL")]
    public string Url { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    public string MediaType { get; set; } = "Video";

    [Range(1, 5)]
    public int Level { get; set; } = 1;

    [Display(Name = "Category")]
    public int CategoryId { get; set; }
    public Category? Category { get; set; }

    [Display(Name = "Created At")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Display(Name = "Updated At")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<Bookmark> Bookmarks { get; set; } = new List<Bookmark>();
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
}
