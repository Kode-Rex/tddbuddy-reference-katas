using System.Text;

namespace TextJustification;

public static class TextJustifier
{
    public static IReadOnlyList<string> Justify(string text, int width)
    {
        var words = text.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries);
        if (words.Length == 0)
        {
            return Array.Empty<string>();
        }

        var lines = new List<string>();
        var lineWords = new List<string>();
        var lineContentLength = 0;

        foreach (var word in words)
        {
            var projected = lineContentLength + lineWords.Count + word.Length;
            if (lineWords.Count > 0 && projected > width)
            {
                lines.Add(JustifyLine(lineWords, lineContentLength, width));
                lineWords.Clear();
                lineContentLength = 0;
            }
            lineWords.Add(word);
            lineContentLength += word.Length;
        }

        if (lineWords.Count > 0)
        {
            lines.Add(string.Join(' ', lineWords));
        }

        return lines;
    }

    private static string JustifyLine(List<string> lineWords, int contentLength, int width)
    {
        if (lineWords.Count == 1)
        {
            var only = lineWords[0];
            return only.Length >= width ? only : only.PadRight(width);
        }

        var gaps = lineWords.Count - 1;
        var padding = width - contentLength;
        var baseSpaces = padding / gaps;
        var extras = padding % gaps;

        var sb = new StringBuilder(width);
        for (var i = 0; i < lineWords.Count; i++)
        {
            sb.Append(lineWords[i]);
            if (i < gaps)
            {
                var spaces = baseSpaces + (i < extras ? 1 : 0);
                sb.Append(' ', spaces);
            }
        }
        return sb.ToString();
    }
}
