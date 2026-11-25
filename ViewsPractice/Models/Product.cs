using System.ComponentModel.DataAnnotations;

namespace ViewsPractice.Models;

public class Product
{
    public int Id { get; set; }
    [Required, StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Range(0.01, 1_000_000)]
    public double Price { get; set; }

    [Required, StringLength(60)]
    public string Category { get; set; } = string.Empty;

    [StringLength(500)]
    public string Description { get; set; } = string.Empty;
}
