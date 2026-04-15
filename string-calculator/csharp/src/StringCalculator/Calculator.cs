using System.Linq;

namespace StringCalculator;

public static class Calculator
{
    public static int Add(string numbers)
    {
        if (numbers == "") return 0;
        return numbers.Split(',').Sum(int.Parse);
    }
}
