using Lab03Products.Models;

namespace Lab03Products.Services;

public interface IProductRepository
{
    IReadOnlyList<Product> GetAll();
    Product? GetById(int id);
    Product Add(Product product);
    bool Update(Product product);
    bool Delete(int id);
}
