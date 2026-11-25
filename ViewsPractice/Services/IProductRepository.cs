using ViewsPractice.Models;

namespace ViewsPractice.Services;

public interface IProductRepository
{
    IReadOnlyList<Product> GetAll();
    Product? GetById(int id);
    Product Add(Product product);
    bool Update(Product product);
    bool Delete(int id);
}
