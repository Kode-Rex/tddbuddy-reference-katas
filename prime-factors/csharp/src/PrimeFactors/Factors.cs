namespace PrimeFactors;

public static class Factors
{
    public static IReadOnlyList<int> Generate(int n)
    {
        var factors = new List<int>();
        while (n % 2 == 0)
        {
            factors.Add(2);
            n /= 2;
        }
        if (n > 1) factors.Add(n);
        return factors;
    }
}
