using System.Text.RegularExpressions;

namespace StringCalculator;

internal static class DelimiterParser
{
    private static readonly string[] DefaultDelimiters = { ",", "\n" };

    public static (string[] Delimiters, string Body) Parse(string input)
    {
        if (!input.StartsWith("//"))
        {
            return (DefaultDelimiters, input);
        }

        var headerEnd = input.IndexOf('\n');
        var header = input.Substring(2, headerEnd - 2);
        var body = input.Substring(headerEnd + 1);

        string[] delimiters;
        if (header.StartsWith("["))
        {
            var matches = Regex.Matches(header, @"\[([^\]]+)\]");
            delimiters = new string[matches.Count];
            for (var i = 0; i < matches.Count; i++)
            {
                delimiters[i] = matches[i].Groups[1].Value;
            }
        }
        else
        {
            delimiters = new[] { header };
        }

        return (delimiters, body);
    }
}
