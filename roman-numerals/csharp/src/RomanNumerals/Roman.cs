using System.Collections.Generic;
using System.Text;

namespace RomanNumerals;

public static class Roman
{
    private static readonly (int Value, string Symbol)[] Mapping = new[]
    {
        (900, "CM"),
        (500, "D"),
        (400, "CD"),
        (100, "C"),
        (90, "XC"),
        (50, "L"),
        (40, "XL"),
        (10, "X"),
        (9, "IX"),
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
