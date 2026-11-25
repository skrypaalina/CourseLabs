using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Lab04ValidationBinding.Models;
using Lab04ValidationBinding.Services;

namespace Lab04ValidationBinding.Controllers;

[Route("users")]
public class UsersController : Controller
{
    private const string AdminParameter = "df2323eoT";
    private readonly IUserRepository users;

    public UsersController(IUserRepository users) => this.users = users;

    [HttpGet("index")]
    public IActionResult Index() => Unauthorized();

    [HttpPost("index")]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        string? parameter = null;
        if (Request.HasFormContentType)
        {
            var form = await Request.ReadFormAsync(cancellationToken);
            parameter = form["parameter"].FirstOrDefault();
        }
        else if (Request.ContentLength is > 0)
        {
            try
            {
                var body = await Request.ReadFromJsonAsync<AdminAccessRequest>(cancellationToken);
                parameter = body?.Parameter;
            }
            catch (JsonException)
            {
                return Unauthorized();
            }
        }

        if (!string.Equals(parameter, AdminParameter, StringComparison.Ordinal))
            return Unauthorized();

        return View(users.GetAll());
    }

    [HttpGet("create")]
    public IActionResult Create([FromQuery] User user, [FromQuery] bool save = false)
    {
        if (!save)
        {
            ModelState.Clear();
            return View(user);
        }

        if (!ModelState.IsValid) return View(user);

        user.Id = 0;
        return View("Details", users.Add(user));
    }

    [HttpGet("{id:int}")]
    public IActionResult Details(int id)
    {
        var user = users.GetById(id);
        return user is null ? NotFound() : View(user);
    }
}
