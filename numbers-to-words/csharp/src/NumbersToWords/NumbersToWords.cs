namespace NumbersToWords;

public static class NumbersToWords
{
    private static readonly string[] Ones =
    {
        "zero", "one", "two", "three", "four",
        "five", "six", "seven", "eight", "nine",
        "ten", "eleven", "twelve", "thirteen", "fourteen",
        "fifteen", "sixteen", "seventeen", "eighteen", "nineteen"
    };

    private static readonly string[] Tens =
    {
        "", "", "twenty", "thirty", "forty",
        "fifty", "sixty", "seventy", "eighty", "ninety"
    };

    public static string ToWords(int n)
    {
        if (n == 0) return "zero";

        var parts = new List<string>();

        var thousands = n / 1000;
        if (thousands > 0)
        {
            parts.Add($"{Ones[thousands]} thousand");
            n %= 1000;
        }

        var hundreds = n / 100;
        if (hundreds > 0)
        {
            parts.Add($"{Ones[hundreds]} hundred");
            n %= 100;
        }

        if (n > 0)
        {
            parts.Add(BelowHundred(n));
        }

        return string.Join(" ", parts);
    }

    private static string BelowHundred(int n)
    {
        if (n < 20) return Ones[n];
        var tens = n / 10;
        var ones = n % 10;
        return ones == 0 ? Tens[tens] : $"{Tens[tens]}-{Ones[ones]}";
    }
}
