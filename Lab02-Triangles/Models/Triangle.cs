using System.Globalization;

public class Triangle
{
    public double Side1 { get; set; }
    public double Side2 { get; set; }
    public double Side3 { get; set; }

    public double[] SortedSides =>
        new[] { Side1, Side2, Side3 }.OrderBy(x => x).ToArray();

    public bool IsValid
    {
        get
        {
            var s = SortedSides;
            return s.All(x => double.IsFinite(x) && x > 0)
                && s[0] + s[1] > s[2];
        }
    }

    public double Perimeter => Side1 + Side2 + Side3;

    public double Area
    {
        get
        {
            var half = Perimeter / 2;
            return Math.Sqrt(half * (half - Side1) * (half - Side2) * (half - Side3));
        }
    }

    // Числа вважаються рівними, якщо різниця менша за 0,001% першого числа.
    public static bool NearlyEqual(double a, double b) =>
        Math.Abs(a - b) < Math.Abs(a) * 0.00001;

    public bool IsEquilateral =>
        NearlyEqual(Side1, Side2) && NearlyEqual(Side2, Side3);

    public bool IsIsosceles =>
        NearlyEqual(Side1, Side2)
        || NearlyEqual(Side1, Side3)
        || NearlyEqual(Side2, Side3);

    public bool IsRightAngled
    {
        get
        {
            var s = SortedSides;
            return NearlyEqual(s[0] * s[0] + s[1] * s[1], s[2] * s[2]);
        }
    }

    public bool IsCongruentTo(Triangle other)
    {
        var a = SortedSides;
        var b = other.SortedSides;
        return Enumerable.Range(0, 3).All(i => NearlyEqual(a[i], b[i]));
    }

    public bool IsSimilarTo(Triangle other)
    {
        var a = SortedSides;
        var b = other.SortedSides;
        return NearlyEqual(a[0] / b[0], a[1] / b[1])
            && NearlyEqual(a[1] / b[1], a[2] / b[2]);
    }

    public string ToInfo()
    {
        var s = SortedSides;
        var culture = CultureInfo.InvariantCulture;

        return $"""
        ------------------------------------------------------------
        Triangle:
        ({s[0]}, {s[1]}, {s[2]})
        Reduced:
        ({s[0] / Perimeter}, {s[1] / Perimeter}, {s[2] / Perimeter})

        Area = {Area.ToString("0.####", culture)}
        Perimeter = {Perimeter.ToString("0.####", culture)}
        ------------------------------------------------------------
        """;
    }
}