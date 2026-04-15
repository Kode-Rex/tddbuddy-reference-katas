using System.Globalization;

namespace FizzBuzzWhiz;

public static class FizzBuzzWhiz
{
    public static string Say(int n)
    {
        if (n % 15 == 0) return "FizzBuzz";
        if (n % 3 == 0) return "Fizz";
        if (n % 5 == 0) return "Buzz";
        return n.ToString(CultureInfo.InvariantCulture);
    }
}
