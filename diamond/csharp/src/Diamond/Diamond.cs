using System.Text;

namespace Diamond;

public static class Diamond
{
    public static string Print(char letter)
    {
        var normalized = char.ToUpperInvariant(letter);
        if (normalized < 'A' || normalized > 'Z')
        {
            throw new ArgumentException(
                "letter must be a single A-Z character",
                nameof(letter));
        }

        var n = normalized - 'A';
        var rows = new List<string>(2 * n + 1);
        for (var r = 0; r <= 2 * n; r++)
        {
            var offset = r <= n ? r : 2 * n - r;
            rows.Add(BuildRow(offset, n));
        }
        return string.Join("\n", rows);
    }

    private static string BuildRow(int offset, int n)
    {
        var leading = n - offset;
        var row = new StringBuilder(2 * n + 1);
        row.Append(' ', leading);
        var rowLetter = (char)('A' + offset);
        row.Append(rowLetter);
        if (offset > 0)
        {
            row.Append(' ', 2 * offset - 1);
            row.Append(rowLetter);
        }
        return row.ToString();
    }
}
