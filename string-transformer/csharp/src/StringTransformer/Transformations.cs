using System.Text;

namespace StringTransformer;

public sealed class Capitalise : ITransformation
{
    public string Apply(string input)
    {
        if (input.Length == 0) return input;
        var chars = input.ToCharArray();
        var atWordStart = true;
        for (var i = 0; i < chars.Length; i++)
        {
            var c = chars[i];
            var isLetter = IsLetter(c);
            if (isLetter && atWordStart)
            {
                chars[i] = char.ToUpperInvariant(c);
            }
            atWordStart = !isLetter;
        }
        return new string(chars);
    }

    private static bool IsLetter(char c) =>
        (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z');
}

public sealed class Reverse : ITransformation
{
    public string Apply(string input)
    {
        var chars = input.ToCharArray();
        Array.Reverse(chars);
        return new string(chars);
    }
}

public sealed class RemoveWhitespace : ITransformation
{
    public string Apply(string input)
    {
        var sb = new StringBuilder(input.Length);
        foreach (var c in input)
        {
            if (!char.IsWhiteSpace(c)) sb.Append(c);
        }
        return sb.ToString();
    }
}

public sealed class SnakeCase : ITransformation
{
    public string Apply(string input)
    {
        var lowered = input.ToLowerInvariant();
        var sb = new StringBuilder(lowered.Length);
        var inSeparatorRun = true; // suppress leading separators
        foreach (var c in lowered)
        {
            if (IsSeparator(c))
            {
                inSeparatorRun = true;
            }
            else
            {
                if (inSeparatorRun && sb.Length > 0) sb.Append('_');
                sb.Append(c);
                inSeparatorRun = false;
            }
        }
        return sb.ToString();
    }

    private static bool IsSeparator(char c) => char.IsWhiteSpace(c) || c == '-';
}

public sealed class CamelCase : ITransformation
{
    public string Apply(string input)
    {
        var sb = new StringBuilder(input.Length);
        var tokenIndex = 0;
        var atTokenStart = true;
        foreach (var c in input)
        {
            if (IsSeparator(c))
            {
                if (!atTokenStart)
                {
                    tokenIndex++;
                    atTokenStart = true;
                }
            }
            else
            {
                var lower = char.ToLowerInvariant(c);
                if (atTokenStart && tokenIndex > 0)
                {
                    sb.Append(char.ToUpperInvariant(lower));
                }
                else
                {
                    sb.Append(lower);
                }
                atTokenStart = false;
            }
        }
        return sb.ToString();
    }

    private static bool IsSeparator(char c) => char.IsWhiteSpace(c) || c == '-';
}

public sealed class Truncate : ITransformation
{
    private readonly int _length;

    public Truncate(int length)
    {
        _length = length;
    }

    public string Apply(string input)
    {
        if (input.Length <= _length) return input;
        return input.Substring(0, _length) + TruncationMarker.Value;
    }
}

public sealed class Repeat : ITransformation
{
    private readonly int _times;

    public Repeat(int times)
    {
        _times = times;
    }

    public string Apply(string input)
    {
        if (_times <= 0) return string.Empty;
        return string.Join(' ', Enumerable.Repeat(input, _times));
    }
}

public sealed class Replace : ITransformation
{
    private readonly string _target;
    private readonly string _replacement;

    public Replace(string target, string replacement)
    {
        _target = target;
        _replacement = replacement;
    }

    public string Apply(string input) => input.Replace(_target, _replacement);
}
