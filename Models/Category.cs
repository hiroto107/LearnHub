using System.ComponentModel.DataAnnotations;

namespace LearnHub.Models;

public class Category
{
    public int Id { get; set; }

    [Required]
    [StringLength(80)]
    public string Name { get; set; } = string.Empty;

    public ICollection<Resource> Resources { get; set; } = new List<Resource>();
}
