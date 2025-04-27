namespace Fractionals;

using Fractional = (long nom, long denom);

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
