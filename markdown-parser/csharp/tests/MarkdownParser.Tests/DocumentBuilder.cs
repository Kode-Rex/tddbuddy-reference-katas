namespace MarkdownParser.Tests;

public class DocumentBuilder
{
    private readonly List<string> _lines = new();

    public DocumentBuilder WithLine(string line) { _lines.Add(line); return this; }
    public DocumentBuilder WithBlankLine() { _lines.Add(""); return this; }
    public DocumentBuilder WithHeading(int level, string text) { _lines.Add($"{new string('#', level)} {text}"); return this; }
    public DocumentBuilder WithParagraph(string text) { _lines.Add(text); return this; }
    public DocumentBuilder WithListItem(string text) { _lines.Add($"- {text}"); return this; }
    public DocumentBuilder WithBlockquote(string text) { _lines.Add($"> {text}"); return this; }
    public DocumentBuilder WithCodeBlock(string content)
    {
        _lines.Add("```");
        _lines.Add(content);
        _lines.Add("```");
        return this;
    }

    public string Build() => string.Join("\n", _lines);
}
