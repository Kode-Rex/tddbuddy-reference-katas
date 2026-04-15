namespace EndOfLineTrim;

public static class EndOfLineTrim
{
    public static string Trim(string input)
    {
        var result = new System.Text.StringBuilder(input.Length);
        var lineStart = 0;
        var i = 0;
        while (i < input.Length)
        {
            if (input[i] == '\n')
            {
                AppendTrimmedLine(result, input, lineStart, i);
                result.Append('\n');
                lineStart = i + 1;
                i++;
            }
            else if (input[i] == '\r' && i + 1 < input.Length && input[i + 1] == '\n')
            {
                AppendTrimmedLine(result, input, lineStart, i);
                result.Append("\r\n");
                lineStart = i + 2;
                i += 2;
            }
            else
            {
                i++;
            }
        }
        AppendTrimmedLine(result, input, lineStart, input.Length);
        return result.ToString();
    }

    private static void AppendTrimmedLine(System.Text.StringBuilder result, string input, int start, int end)
    {
        var trimmedEnd = end;
        while (trimmedEnd > start && (input[trimmedEnd - 1] == ' ' || input[trimmedEnd - 1] == '\t'))
        {
            trimmedEnd--;
        }
        result.Append(input, start, trimmedEnd - start);
    }
}
