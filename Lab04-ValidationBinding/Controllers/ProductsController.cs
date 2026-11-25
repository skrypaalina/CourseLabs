using Microsoft.AspNetCore.Mvc;
using Lab04ValidationBinding.Models;
using Lab04ValidationBinding.Services;

namespace Lab04ValidationBinding.Controllers;

[Route("products")]
[Route("items")]
public class ProductsController(IProductRepository products) : Controller
{
    [HttpGet("")]
    [HttpGet("index")]
    public IActionResult Index(string? name, string? category, double? minPrice, double? maxPrice)
    {
        var allProducts = products.GetAll();
        var filtered = allProducts.AsEnumerable();
        if (!string.IsNullOrWhiteSpace(name))
            filtered = filtered.Where(p => p.Name.Contains(name, StringComparison.OrdinalIgnoreCase));
        if (!string.IsNullOrWhiteSpace(category))
            filtered = filtered.Where(p => p.Category.Contains(category, StringComparison.OrdinalIgnoreCase));
        if (minPrice.HasValue) filtered = filtered.Where(p => p.Price >= minPrice.Value);
        if (maxPrice.HasValue) filtered = filtered.Where(p => p.Price <= maxPrice.Value);

        return View(new ProductIndexViewModel
        {
            Products = filtered.ToList(),
            Name = name,
            Category = category,
            MinPrice = minPrice,
            MaxPrice = maxPrice,
            Categories = allProducts.Select(p => p.Category).Distinct(StringComparer.OrdinalIgnoreCase).OrderBy(c => c).ToList()
        });
    }

    [HttpGet("edit-prices/{category}")]
    public IActionResult EditPrices(string category)
    {
        var categoryProducts = GetCategoryProducts(category);
        if (categoryProducts.Count == 0) return ProductNotFound();

        return View(new BulkPriceUpdateViewModel
        {
            Category = category,
            Products = categoryProducts.Select(p => new BulkPriceRowViewModel { Id = p.Id, Name = p.Name, Price = p.Price }).ToList()
        });
    }

    [HttpPost("edit-prices/{category}")]
    [ValidateAntiForgeryToken]
    public IActionResult EditPrices(string category, BulkPriceUpdateViewModel model)
    {
        var categoryProducts = GetCategoryProducts(category);
        if (categoryProducts.Count == 0) return ProductNotFound();

        var expectedIds = categoryProducts.Select(p => p.Id).OrderBy(id => id).ToArray();
        var submittedIds = model.Products.Select(p => p.Id).OrderBy(id => id).ToArray();
        if (!expectedIds.SequenceEqual(submittedIds))
            ModelState.AddModelError(nameof(model.Products), "The product list has changed. Reload the page and try again.");

        model.Category = category;
        foreach (var row in model.Products)
        {
            var current = categoryProducts.FirstOrDefault(p => p.Id == row.Id);
            if (current is not null) row.Name = current.Name;
        }

        if (!ModelState.IsValid) return View(model);

        foreach (var row in model.Products)
        {
            var product = categoryProducts.First(p => p.Id == row.Id);
            product.Price = row.Price!.Value;
            products.Update(product);
        }

        return RedirectToAction(nameof(Index), new { category });
    }

    [HttpGet("{id:int}")]
    [HttpGet("details/{id:int}")]
    public IActionResult Details(int id)
    {
        var product = products.GetById(id);
        return product is null ? ProductNotFound() : View(product);
    }

    [HttpGet("create")]
    [HttpGet("new")]
    public IActionResult Create() => View(new Product());

    [HttpPost("create")]
    [HttpPost("new")]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Product product)
    {
        if (!ModelState.IsValid) return View(product);
        var created = products.Add(product);
        return RedirectToAction(nameof(Details), new { id = created.Id });
    }

    [HttpGet("edit/{id:int}")]
    public IActionResult Edit(int id)
    {
        var product = products.GetById(id);
        return product is null ? ProductNotFound() : View(product);
    }

    [HttpPost("edit/{id:int}")]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, Product product)
    {
        if (id != product.Id) return BadRequest();
        if (!ModelState.IsValid) return View(product);
        return products.Update(product) ? RedirectToAction(nameof(Details), new { id }) : ProductNotFound();
    }

    [HttpGet("delete/{id:int}")]
    public IActionResult Delete(int id)
    {
        var product = products.GetById(id);
        return product is null ? ProductNotFound() : View(product);
    }

    [HttpPost("delete/{id:int}")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        return products.Delete(id) ? RedirectToAction(nameof(Index)) : ProductNotFound();
    }

    private IActionResult ProductNotFound()
    {
        Response.StatusCode = StatusCodes.Status404NotFound;
        return View("ProductNotFound");
    }

    private List<Product> GetCategoryProducts(string category) => products.GetAll()
        .Where(p => string.Equals(p.Category, category, StringComparison.OrdinalIgnoreCase))
        .OrderBy(p => p.Id)
        .ToList();
}
