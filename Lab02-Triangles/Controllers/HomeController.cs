using Microsoft.AspNetCore.Mvc;

public class HomeController : Controller
{
    private readonly IGreetingService _greetingService;

    public HomeController(IGreetingService greetingService)
    {
        _greetingService = greetingService;
    }

    public IActionResult Index()
    {
        ViewBag.Message = _greetingService.GetWelcomeMessage();
        return View();
    }
}