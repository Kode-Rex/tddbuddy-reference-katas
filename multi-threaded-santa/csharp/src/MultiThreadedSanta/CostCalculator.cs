namespace MultiThreadedSanta;

/// <summary>
/// Calculates the cookie cost: elves x elapsed seconds = cookies.
/// </summary>
public static class CostCalculator
{
    public static double CalculateCookies(int elfCount, TimeSpan elapsed)
    {
        return elfCount * elapsed.TotalSeconds;
    }
}
