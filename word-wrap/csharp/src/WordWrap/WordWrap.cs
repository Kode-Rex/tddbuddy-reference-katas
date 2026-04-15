using System.Text;

namespace WordWrap;

public static class WordWrap
{
    public static string Wrap(string text, int width)
    {
        var words = text.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries);
        if (words.Length == 0)
        {
            return string.Empty;
        }

        var lines = new List<string>();
        var line = new StringBuilder();

        foreach (var original in words)
        {
            var word = original;
            while (word.Length > width)
            {
                if (line.Length > 0)
                {
                    lines.Add(line.ToString());
                    line.Clear();
                }
                lines.Add(word[..width]);
                word = word[width..];
            }

            if (line.Length == 0)
            {
                line.Append(word);
            }
            else if (line.Length + 1 + word.Length <= width)
            {
                line.Append(' ').Append(word);
            }
            else
            {
                lines.Add(line.ToString());
                line.Clear();
                line.Append(word);
            }
        }

        if (line.Length > 0)
        {
            lines.Add(line.ToString());
        }

        return string.Join('\n', lines);
    }
}
