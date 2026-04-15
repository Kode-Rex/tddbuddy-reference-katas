using System.Linq;

namespace StringCalculator;

public static class Calculator
{
    public static int Add(string numbers)
    {
        if (numbers == "") return 0;

        var delimiters = new[] { ",", "\n" };
        var body = numbers;

        if (numbers.StartsWith("//"))
        {
            var headerEnd = numbers.IndexOf('\n');
            var header = numbers.Substring(2, headerEnd - 2);
            delimiters = new[] { header };
            body = numbers.Substring(headerEnd + 1);
        }

        return body.Split(delimiters, System.StringSplitOptions.None).Sum(int.Parse);
    }
}
