using System.ComponentModel.DataAnnotations;

namespace Lab04ValidationBinding.Models;

public class BulkPriceUpdateViewModel
{
    public string Category { get; set; } = string.Empty;
    public List<BulkPriceRowViewModel> Products { get; set; } = [];
}

public class BulkPriceRowViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    [Required]
    [Range(0, 100_000, ErrorMessage = "Price must be between 0 and 100000.")]
    public double? Price { get; set; }
}
