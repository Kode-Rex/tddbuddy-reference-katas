using System.Linq;

namespace StringCalculator;

public static class Calculator
{
    public static int Add(string numbers)
    {
        if (numbers == "") return 0;
        var (delimiters, body) = DelimiterParser.Parse(numbers);
        return body.Split(delimiters, System.StringSplitOptions.None).Sum(int.Parse);
    }
}
