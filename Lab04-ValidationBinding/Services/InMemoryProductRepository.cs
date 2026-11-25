using Lab04ValidationBinding.Models;

namespace Lab04ValidationBinding.Services;

public class InMemoryProductRepository : IProductRepository
{
    private readonly object _lock = new();
    private readonly List<Product> _products =
    [
        new() { Id = 1, Name = "Teddy Bear", Price = 18.49, Category = "Toys", Description = "Teddy Bear with soft fabric", Type = ProductType.Toy },
        new() { Id = 2, Name = "Coffee Machine", Price = 84.25, Category = "Appliances", Description = "Coffee Machine with timer", Type = ProductType.Technique },
        new() { Id = 3, Name = "T-shirt", Price = 23.79, Category = "Apparel", Description = "T-shirt made from cotton", Type = ProductType.Clothes },
        new() { Id = 4, Name = "Toy Car", Price = 16.10, Category = "Toys", Description = "Toy Car with rechargeable battery", Type = ProductType.Transport }
    ];
    private int _nextId = 5;

    public IReadOnlyList<Product> GetAll()
    {
        lock (_lock) return _products.Select(Copy).ToList();
    }

    public Product? GetById(int id)
    {
        lock (_lock)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            return product is null ? null : Copy(product);
        }
    }

    public Product Add(Product product)
    {
        lock (_lock)
        {
            product.Id = _nextId++;
            _products.Add(Copy(product));
            return Copy(product);
        }
    }

    public bool Update(Product product)
    {
        lock (_lock)
        {
            var index = _products.FindIndex(p => p.Id == product.Id);
            if (index < 0) return false;
            _products[index] = Copy(product);
            return true;
        }
    }

    public bool Delete(int id)
    {
        lock (_lock)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            return product is not null && _products.Remove(product);
        }
    }

    private static Product Copy(Product product) => new()
    {
        Id = product.Id,
        Name = product.Name,
        Price = product.Price,
        Category = product.Category,
        Description = product.Description,
        Type = product.Type
    };
}
