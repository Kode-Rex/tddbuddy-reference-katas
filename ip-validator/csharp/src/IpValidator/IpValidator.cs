namespace IpValidator;

public static class IpValidator
{
    public static bool IsValid(string input)
    {
        if (string.IsNullOrEmpty(input)) return false;

        var octets = input.Split('.');
        if (octets.Length != 4) return false;

        for (var i = 0; i < 4; i++)
        {
            if (!TryParseOctet(octets[i], out var value)) return false;
            if (i == 3 && (value == 0 || value == 255)) return false;
        }

        return true;
    }

    private static bool TryParseOctet(string octet, out int value)
    {
        value = 0;
        if (octet.Length == 0) return false;
        if (octet.Length > 1 && octet[0] == '0') return false;

        foreach (var c in octet)
        {
            if (c < '0' || c > '9') return false;
            value = value * 10 + (c - '0');
            if (value > 255) return false;
        }

        return true;
    }
}
