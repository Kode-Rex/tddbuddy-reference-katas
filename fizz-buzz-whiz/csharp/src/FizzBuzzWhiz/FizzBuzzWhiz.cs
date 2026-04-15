using System.Globalization;

namespace FizzBuzzWhiz;

public static class FizzBuzzWhiz
{
    public static string Say(int n)
    {
        var divisibleByThree = n % 3 == 0;
        var divisibleByFive = n % 5 == 0;

        if (divisibleByThree && divisibleByFive) return "FizzBuzz";
        if (divisibleByThree) return "Fizz";
        if (divisibleByFive) return "Buzz";
        return n.ToString(CultureInfo.InvariantCulture);
    }
}
