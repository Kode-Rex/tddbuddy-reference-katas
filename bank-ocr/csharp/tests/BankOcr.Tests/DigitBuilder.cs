namespace BankOcr.Tests;

/// <summary>
/// Builds a 3x3 OCR digit grid. Defaults to the blank glyph ("   ", "   ", "   ").
/// Use <see cref="ForDigit"/> for canonical glyphs or <see cref="WithRow"/> to override specific rows
/// (useful for crafting malformed / unknown digits in tests).
/// </summary>
public class DigitBuilder
{
    private static readonly IReadOnlyDictionary<int, string[]> Canonical = new Dictionary<int, string[]>
    {
        [0] = new[] { " _ ", "| |", "|_|" },
        [1] = new[] { "   ", "  |", "  |" },
        [2] = new[] { " _ ", " _|", "|_ " },
        [3] = new[] { " _ ", " _|", " _|" },
        [4] = new[] { "   ", "|_|", "  |" },
        [5] = new[] { " _ ", "|_ ", " _|" },
        [6] = new[] { " _ ", "|_ ", "|_|" },
        [7] = new[] { " _ ", "  |", "  |" },
        [8] = new[] { " _ ", "|_|", "|_|" },
        [9] = new[] { " _ ", "|_|", " _|" },
    };

    private string[] _rows = { "   ", "   ", "   " };

    public DigitBuilder ForDigit(int value)
    {
        _rows = (string[])Canonical[value].Clone();
        return this;
    }

    public DigitBuilder WithRow(int index, string row)
    {
        _rows[index] = row;
        return this;
    }

    public string[] Build() => (string[])_rows.Clone();
}
