using System;
using System.Linq;

namespace StringCalculator;

public static class Calculator
{
    public static int Add(string numbers)
    {
        if (numbers == "") return 0;
        var (delimiters, body) = DelimiterParser.Parse(numbers);
        var parsed = body.Split(delimiters, StringSplitOptions.None).Select(int.Parse).ToArray();
        var negatives = parsed.Where(n => n < 0).ToArray();
        if (negatives.Length > 0)
        {
            throw new ArgumentException("negatives not allowed: " + string.Join(", ", negatives));
        }
        return parsed.Where(n => n <= 1000).Sum();
    }
}
