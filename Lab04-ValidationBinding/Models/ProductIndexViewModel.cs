namespace Lab04ValidationBinding.Models;

public class ProductIndexViewModel
{
    public IReadOnlyList<Product> Products { get; set; } = [];
    public string? Name { get; set; }
    public string? Category { get; set; }
    public double? MinPrice { get; set; }
    public double? MaxPrice { get; set; }
    public IReadOnlyList<string> Categories { get; set; } = [];
}
