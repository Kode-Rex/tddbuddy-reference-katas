namespace PrimeFactors;

public static class Factors
{
    public static IReadOnlyList<int> Generate(int n)
    {
        var factors = new List<int>();
        for (var divisor = 2; n > 1; divisor++)
        {
            while (n % divisor == 0)
            {
                factors.Add(divisor);
                n /= divisor;
            }
        }
        return factors;
    }
}
