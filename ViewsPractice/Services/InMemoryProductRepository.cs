using ViewsPractice.Models;

namespace ViewsPractice.Services;

public class InMemoryProductRepository : IProductRepository
{
    private readonly object _lock = new();
    private readonly List<Product> _products =
    [
        new() { Id = 1, Name = "Coffee", Price = 8.49, Category = "Drinks", Description = "Ground coffee, 250 g" },
        new() { Id = 2, Name = "Green tea", Price = 4.25, Category = "Drinks", Description = "Loose leaf tea, 100 g" },
        new() { Id = 3, Name = "Dark chocolate", Price = 3.79, Category = "Sweets", Description = "Dark chocolate, 100 g" },
        new() { Id = 4, Name = "Honey", Price = 6.10, Category = "Pantry", Description = "Wildflower honey, 500 g" }
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
        Description = product.Description
    };
}
