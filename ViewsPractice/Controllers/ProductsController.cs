using Microsoft.AspNetCore.Mvc;
using ViewsPractice.Models;
using ViewsPractice.Services;

namespace ViewsPractice.Controllers;

[Route("products")]
[Route("items")]
public class ProductsController(IProductRepository products) : Controller
{
    [HttpGet("")]
    [HttpGet("index")]
    public IActionResult Index(string? name, string? category, double? minPrice, double? maxPrice)
    {
        var filtered = products.GetAll().AsEnumerable();
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
            MaxPrice = maxPrice
        });
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
}
