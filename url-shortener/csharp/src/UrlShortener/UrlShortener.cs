namespace UrlShortener;

public record UrlStatistics(string ShortUrl, string LongUrl, int Visits);

public class Shortener
{
    private const string ShortUrlBase = "https://short.url/";

    private readonly Dictionary<string, string> _longToCode = new();
    private readonly Dictionary<string, string> _codeToLong = new();
    private readonly Dictionary<string, int> _visits = new();
    private int _nextCounter;

    public string Shorten(string longUrl)
    {
        if (_longToCode.TryGetValue(longUrl, out var existingCode))
        {
            return ShortUrlBase + existingCode;
        }

        var code = ToBase36(_nextCounter++);
        _longToCode[longUrl] = code;
        _codeToLong[code] = longUrl;
        _visits[code] = 0;
        return ShortUrlBase + code;
    }

    public string Translate(string url)
    {
        if (TryResolveByShortUrl(url, out var shortCode))
        {
            _visits[shortCode]++;
            return ShortUrlBase + shortCode;
        }

        if (_longToCode.TryGetValue(url, out var codeFromLong))
        {
            return ShortUrlBase + codeFromLong;
        }

        throw new ArgumentException($"Unknown URL: {url}");
    }

    public UrlStatistics Statistics(string url)
    {
        if (TryResolveByShortUrl(url, out var shortCode))
        {
            return BuildStatistics(shortCode);
        }

        if (_longToCode.TryGetValue(url, out var codeFromLong))
        {
            return BuildStatistics(codeFromLong);
        }

        throw new ArgumentException($"Unknown URL: {url}");
    }

    private bool TryResolveByShortUrl(string url, out string shortCode)
    {
        if (url.StartsWith(ShortUrlBase, StringComparison.Ordinal))
        {
            var candidate = url[ShortUrlBase.Length..];
            if (_codeToLong.ContainsKey(candidate))
            {
                shortCode = candidate;
                return true;
            }
        }
        shortCode = "";
        return false;
    }

    private UrlStatistics BuildStatistics(string code) =>
        new(ShortUrlBase + code, _codeToLong[code], _visits[code]);

    private static string ToBase36(int value)
    {
        if (value == 0) return "0";
        const string alphabet = "0123456789abcdefghijklmnopqrstuvwxyz";
        var digits = new Stack<char>();
        while (value > 0)
        {
            digits.Push(alphabet[value % 36]);
            value /= 36;
        }
        return new string(digits.ToArray());
    }
}
