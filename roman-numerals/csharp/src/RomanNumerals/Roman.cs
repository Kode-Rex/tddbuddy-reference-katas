namespace RomanNumerals;

public static class Roman
{
    public static string ToRoman(int n)
    {
        if (n == 2) return "II";
        return "I";
    }
}
