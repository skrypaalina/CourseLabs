using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ViewsPractice.Models;

namespace ViewsPractice.Controllers;

public class HomeController : Controller
{
    private static readonly Dictionary<string, int> ShoppingItems = new()
    {
        ["Apples"] = 4,
        ["Bread"] = 1,
        ["Milk"] = 2,
        ["Coffee"] = 1
    };

    private static readonly List<string> Markets = ["ATB", "Silpo", "Novus", "Auchan"];

    public IActionResult Index() => View();
    public IActionResult SprintTasks() => View();
    public IActionResult Greetings() => View();
    public IActionResult ProductInfo() => View();

    public IActionResult SuperMarkets()
    {
        ViewBag.SuperMarkets = Markets;
        return View();
    }

    public IActionResult ShoppingList() => View(ShoppingItems);

    [HttpGet]
    public IActionResult ShoppingCart()
    {
        var model = CreateShoppingCartModel();
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult ShoppingCart(ShoppingCartViewModel model)
    {
        model.Supermarkets = Markets.Select(x => new SelectListItem(x, x)).ToList();
        model.Products = ShoppingItems.Keys.Select(x => new SelectListItem(x, x)).ToList();
        model.DeliveryDates = Enumerable.Range(0, 3).Select(i => DateTime.Today.AddDays(i).ToString("yyyy-MM-dd")).ToList();
        ViewBag.OrderMessage = $"Thank you, {model.CustomerName}! Your order from {model.Supermarket} is scheduled for {model.DeliveryDate}.";
        return View("ShoppingCartResult", model);
    }

    private static ShoppingCartViewModel CreateShoppingCartModel() => new()
    {
        Supermarkets = Markets.Select(x => new SelectListItem(x, x)).ToList(),
        Products = ShoppingItems.Keys.Select(x => new SelectListItem(x, x)).ToList(),
        DeliveryDates = Enumerable.Range(0, 3).Select(i => DateTime.Today.AddDays(i).ToString("yyyy-MM-dd")).ToList()
    };
}
