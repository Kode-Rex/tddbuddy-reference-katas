namespace PrimeFactors;

public static class Factors
{
    public static IReadOnlyList<int> Generate(int n)
    {
        if (n > 1) return new[] { n };
        return Array.Empty<int>();
    }
}
