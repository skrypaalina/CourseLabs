using Microsoft.AspNetCore.Mvc;
using System.Globalization;

[Route("triangle")]
public class TriangleController : Controller
{
    private ContentResult Plain(string value, int statusCode = 200) =>
        new()
        {
            Content = value,
            ContentType = "text/plain; charset=utf-8",
            StatusCode = statusCode
        };

    private ContentResult InvalidTriangle() =>
        Plain("Помилка: задані сторони не утворюють трикутник.", 400);

    private ContentResult InvalidArray() =>
        Plain("Помилка: передайте масив коректних трикутників.", 400);

    private static Triangle Make(double a, double b, double c) =>
        new() { Side1 = a, Side2 = b, Side3 = c };

    [HttpGet("Info")]
    public IActionResult Info(double side1, double side2, double side3)
    {
        var t = Make(side1, side2, side3);
        return t.IsValid ? Plain(t.ToInfo()) : InvalidTriangle();
    }

    [HttpGet("Area")]
    public IActionResult Area(double side1, double side2, double side3)
    {
        var t = Make(side1, side2, side3);
        return t.IsValid
            ? Plain(t.Area.ToString("0.####", CultureInfo.InvariantCulture))
            : InvalidTriangle();
    }

    [HttpGet("Perimeter")]
    public IActionResult Perimeter(double side1, double side2, double side3)
    {
        var t = Make(side1, side2, side3);
        return t.IsValid
            ? Plain(t.Perimeter.ToString("0.####", CultureInfo.InvariantCulture))
            : InvalidTriangle();
    }

    [HttpGet("IsRightAngled")]
    public IActionResult IsRightAngled(double side1, double side2, double side3)
    {
        var t = Make(side1, side2, side3);
        return t.IsValid
            ? Plain(t.IsRightAngled.ToString().ToLowerInvariant())
            : InvalidTriangle();
    }

    [HttpGet("IsEquilateral")]
    public IActionResult IsEquilateral(double side1, double side2, double side3)
    {
        var t = Make(side1, side2, side3);
        return t.IsValid
            ? Plain(t.IsEquilateral.ToString().ToLowerInvariant())
            : InvalidTriangle();
    }

    [HttpGet("IsIsosceles")]
    public IActionResult IsIsosceles(double side1, double side2, double side3)
    {
        var t = Make(side1, side2, side3);
        return t.IsValid
            ? Plain(t.IsIsosceles.ToString().ToLowerInvariant())
            : InvalidTriangle();
    }

    [HttpGet("arecongruent")]
    public IActionResult AreCongruent([FromQuery] Triangle tr1, [FromQuery] Triangle tr2)
    {
        if (!tr1.IsValid || !tr2.IsValid)
            return InvalidTriangle();

        return Plain(tr1.IsCongruentTo(tr2).ToString().ToLowerInvariant());
    }

    [HttpGet("aresimilar")]
    public IActionResult AreSimilar([FromQuery] Triangle tr1, [FromQuery] Triangle tr2)
    {
        if (!tr1.IsValid || !tr2.IsValid)
            return InvalidTriangle();

        return Plain(tr1.IsSimilarTo(tr2).ToString().ToLowerInvariant());
    }

    [HttpGet("GreatesByPerimeter")]
    [HttpGet("GreatestByPerimeter")]
    public IActionResult GreatestByPerimeter([FromQuery(Name = "tr")] Triangle[] tr)
    {
        if (!ValidArray(tr))
            return InvalidArray();

        return Plain(tr.OrderByDescending(t => t.Perimeter).First().ToInfo());
    }

    [HttpGet("GreatestByArea")]
    public IActionResult GreatestByArea([FromQuery(Name = "tr")] Triangle[] tr)
    {
        if (!ValidArray(tr))
            return InvalidArray();

        return Plain(tr.OrderByDescending(t => t.Area).First().ToInfo());
    }

    [HttpGet("PairwiseNonSimilar")]
    public IActionResult PairwiseNonSimilar([FromQuery(Name = "tr")] Triangle[] tr)
    {
        if (!ValidArray(tr))
            return InvalidArray();

        var selected = new List<Triangle>();

        foreach (var t in tr)
        {
            if (selected.All(existing => !existing.IsSimilarTo(t)))
                selected.Add(t);
        }

        return Plain(string.Join(
            Environment.NewLine + Environment.NewLine,
            selected.Select(t => t.ToInfo())));
    }

    private static bool ValidArray(Triangle[]? triangles) =>
        triangles is { Length: > 0 }
        && triangles.All(t => t is not null && t.IsValid);
}