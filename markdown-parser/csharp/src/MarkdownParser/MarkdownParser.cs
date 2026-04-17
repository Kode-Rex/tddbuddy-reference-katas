using System.Text;
using System.Text.RegularExpressions;

namespace MarkdownParser;

public static class MarkdownParser
{
    public static string Parse(string markdown)
    {
        if (string.IsNullOrEmpty(markdown))
            return string.Empty;

        var lines = markdown.Split('\n');
        var blocks = new List<string>();
        var i = 0;

        while (i < lines.Length)
        {
            var line = lines[i];

            // Code block (fenced)
            if (line.TrimStart().StartsWith("```"))
            {
                i++;
                var codeLines = new List<string>();
                while (i < lines.Length && !lines[i].TrimStart().StartsWith("```"))
                {
                    codeLines.Add(lines[i]);
                    i++;
                }
                if (i < lines.Length) i++; // skip closing fence
                blocks.Add($"<pre><code>{string.Join("\n", codeLines)}</code></pre>");
                continue;
            }

            // Blank line
            if (string.IsNullOrWhiteSpace(line))
            {
                i++;
                continue;
            }

            // Heading
            if (TryParseHeading(line, out var heading))
            {
                blocks.Add(heading);
                i++;
                continue;
            }

            // Unordered list
            if (line.StartsWith("- "))
            {
                var items = new List<string>();
                while (i < lines.Length && lines[i].StartsWith("- "))
                {
                    items.Add($"<li>{ApplyInlineFormatting(lines[i][2..])}</li>");
                    i++;
                }
                blocks.Add($"<ul>{string.Join("", items)}</ul>");
                continue;
            }

            // Blockquote
            if (line.StartsWith("> "))
            {
                var content = ApplyInlineFormatting(line[2..]);
                blocks.Add($"<blockquote>{content}</blockquote>");
                i++;
                continue;
            }

            // Paragraph (consecutive non-special lines)
            {
                var paragraphLines = new List<string>();
                while (i < lines.Length
                    && !string.IsNullOrWhiteSpace(lines[i])
                    && !lines[i].StartsWith("- ")
                    && !lines[i].StartsWith("> ")
                    && !lines[i].TrimStart().StartsWith("```")
                    && !IsHeading(lines[i]))
                {
                    paragraphLines.Add(lines[i]);
                    i++;
                }
                var text = string.Join(" ", paragraphLines);
                blocks.Add($"<p>{ApplyInlineFormatting(text)}</p>");
            }
        }

        return string.Join("", blocks);
    }

    private static bool TryParseHeading(string line, out string result)
    {
        var match = Regex.Match(line, @"^(#{1,6}) (.+)$");
        if (match.Success)
        {
            var level = match.Groups[1].Value.Length;
            var content = ApplyInlineFormatting(match.Groups[2].Value);
            result = $"<h{level}>{content}</h{level}>";
            return true;
        }
        result = string.Empty;
        return false;
    }

    private static bool IsHeading(string line)
    {
        return Regex.IsMatch(line, @"^#{1,6} .+$");
    }

    private const string EscapedAsterisk = "\uFFF0";
    private const string EscapedUnderscore = "\uFFF1";
    private const string EscapedBacktick = "\uFFF2";

    internal static string ApplyInlineFormatting(string text)
    {
        // Process escapes first — replace \* \_ \` with placeholders
        text = text.Replace("\\*", EscapedAsterisk);
        text = text.Replace("\\_", EscapedUnderscore);
        text = text.Replace("\\`", EscapedBacktick);

        // Inline code — extract spans before other formatting to preserve verbatim content
        var codeSpans = new List<string>();
        text = Regex.Replace(text, @"`([^`]+)`", m =>
        {
            codeSpans.Add(m.Groups[1].Value);
            return $"\uFFF3{codeSpans.Count - 1}\uFFF3";
        });

        // Bold
        text = Regex.Replace(text, @"\*\*(.+?)\*\*", "<strong>$1</strong>");

        // Italic
        text = Regex.Replace(text, @"_(.+?)_", "<em>$1</em>");

        // Links
        text = Regex.Replace(text, @"\[([^\]]+)\]\(([^)]+)\)", "<a href=\"$2\">$1</a>");

        // Restore inline code spans
        text = Regex.Replace(text, @"\uFFF3(\d+)\uFFF3", m =>
            $"<code>{codeSpans[int.Parse(m.Groups[1].Value)]}</code>");

        // Restore escaped characters
        text = text.Replace(EscapedAsterisk, "*");
        text = text.Replace(EscapedUnderscore, "_");
        text = text.Replace(EscapedBacktick, "`");

        return text;
    }
}
