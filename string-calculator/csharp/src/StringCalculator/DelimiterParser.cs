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
        return (new[] { header }, body);
    }
}
