using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using ViewsPractice.Models;

namespace ViewsPractice.Controllers;

[Route("users")]
public class UsersController : Controller
{
    private const string AdminParameter = "df2323eoT";
    private static readonly string[] Users = ["Alice Johnson", "David Smith", "Maria Brown"];

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

        return View(Users);
    }
}
