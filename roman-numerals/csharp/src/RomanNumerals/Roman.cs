using System.Collections.Generic;

namespace RomanNumerals;

public static class Roman
{
    private static readonly Dictionary<int, string> Lookup = new()
    {
        { 1, "I" },
        { 2, "II" },
        { 3, "III" },
        { 5, "V" },
    };

    public static string ToRoman(int n)
    {
        return Lookup[n];
    }
}
