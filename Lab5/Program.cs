using Fractional = (long nom, long denom);

Console.WriteLine($"""
    # Normalize Test
        Input       {"Output",-11} Expected
        4/6         {(4L, 6L).Normalize().ToStringImproper(),-11} 2/3
        6/5         {(6L, 4L).Normalize().ToStringImproper(),-11} 3/2
        6/-4        {(6L, -4L).Normalize().ToStringImproper(),-11} -3/2
        -6/4        {(-6L, 4L).Normalize().ToStringImproper(),-11} -3/2
        -6/-4       {(-6L, -4L).Normalize().ToStringImproper(),-11} 3/2
    """);

Console.WriteLine($"""
    # ToStringProper Test (not normalized)
        Input       {"Output",-11} Expected
        3/1         {(3L, 1L).ToStringProper(),-11} 3
        3/-1        {(3L, -1L).ToStringProper(),-11} -3
        -3/1        {(-3L, 1L).ToStringProper(),-11} -3
        -3/-1       {(-3L, -1L).ToStringProper(),-11} 3
        3/2         {(3L, 2L).ToStringProper(),-11} 1 + 1/2
        3/-2        {(3L, -2L).ToStringProper(),-11} -1 + 1/-2
        -3/2        {(-3L, 2L).ToStringProper(),-11} -1 - 1/2
        -3/-2       {(-3L, -2L).ToStringProper(),-11} 1 - 1/-2
    """);

Console.WriteLine("# CalcExpr1 Test");

for (int i = 0; i < 6; i++)
{
    Fractional x = CalcExpr1(i).Normalize();
    Fractional y = ((long)i, i + 1L).Normalize(); // n/(n+1)

    string myImpl = $"CalcExpr1 => {x.ToStringImproper()} ({x.ToDouble()})";
    string check = $"n/(n+1) => {y.ToStringImproper()} ({y.ToDouble()})";
    Console.WriteLine($"    n={i}: {myImpl,-44} {check}");
}

Console.WriteLine("# CalcExpr2 Test");

for (int i = 0; i < 6; i++)
{
    Fractional x = CalcExpr2(i).Normalize();
    Fractional y = (i + 1L, 2L * i).Normalize(); // (n+1)/(2n)

    string myImpl = $"CalcExpr2 => {x.ToStringImproper()} ({x.ToDouble()})";
    string check = $"(n+1)/(2n) => {y.ToStringImproper()} ({y.ToDouble()})";
    Console.WriteLine($"    n={i}: {myImpl,-44} {check}");
}

static Fractional CalcExpr1(int n)
{
    if (n < 1)
        return (0, 1);

    Fractional result = (1, 2);

    for (int i = 2; i <= n; ++i)
    {
        Fractional next = (1, i * (i + 1));
        result = result.Plus(next);
    }

    return result;
}

static Fractional CalcExpr2(int n)
{
    if (n < 2)
        return (1, 1);

    Fractional result = (1, 1);

    for (int i = 2; i <= n; ++i)
    {
        // (1 - 1/k^2) = (k^2 - 1) / k^2
        long square = (long)i * i;
        Fractional next = (square - 1, square);
        result = result.Multiply(next);
    }

    return result;
}

static class FractionalExt
{
    public static double ToDouble(this Fractional x)
    {
        if (x.denom == 0)
            return double.PositiveInfinity;

        return x.nom / (double)x.denom;
    }

    public static string ToStringImproper(this Fractional x)
    {
        return $"{x.nom}/{x.denom}";
    }

    public static string ToStringProper(this Fractional x)
    {
        long integral = x.nom / x.denom;
        x.nom %= x.denom;

        if (integral == 0)
        {
            return x.ToStringImproper();
        }

        if (x.nom == 0)
        {
            return $"{integral}";
        }

        char sign = x.nom >= 0 ? '+' : '-';

        return $"{integral} {sign} {Math.Abs(x.nom)}/{x.denom}";
    }

    public static Fractional Normalize(this Fractional x)
    {
        // Greatest Common Divisor
        static long GCD(long a, long b)
        {
            a = Math.Abs(a);
            b = Math.Abs(b);
            while (b != 0)
            {
                (a, b) = (b, a % b);
            }
            return a;
        }

        if (x.denom == 0)
        {
            // 1/0 is Inf, 0/0 is NaN.
            return (1, 0);
        }

        long gcd = GCD(x.nom, x.denom);

        return (x.nom / gcd * (x.denom < 0 ? -1 : 1), Math.Abs(x.denom / gcd));
    }

    public static Fractional Plus(this Fractional a, Fractional b)
    {
        a = a.Normalize();
        b = b.Normalize();
        return Normalize((a.nom * b.denom + a.denom * b.nom, a.denom * b.denom));
    }

    public static Fractional Minus(this Fractional a, Fractional b)
    {
        a = a.Normalize();
        b = b.Normalize();
        return Normalize((a.nom * b.denom - a.denom * b.nom, a.denom * b.denom));
    }

    public static Fractional Multiply(this Fractional a, Fractional b)
    {
        a = a.Normalize();
        b = b.Normalize();
        return Normalize((a.nom * b.nom, a.denom * b.denom));
    }

    public static Fractional Divide(this Fractional a, Fractional b)
    {
        return Multiply(a, (b.denom, b.nom));
    }
}
