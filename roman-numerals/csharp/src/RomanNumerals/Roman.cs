using System.Collections.Generic;
using System.Text;

namespace RomanNumerals;

public static class Roman
{
    private static readonly (int Value, string Symbol)[] Mapping = new[]
    {
        (5, "V"),
        (4, "IV"),
        (1, "I"),
    };

    public static string ToRoman(int n)
    {
        var result = new StringBuilder();
        foreach (var (value, symbol) in Mapping)
        {
            while (n >= value)
            {
                result.Append(symbol);
                n -= value;
            }
        }
        return result.ToString();
    }
}
