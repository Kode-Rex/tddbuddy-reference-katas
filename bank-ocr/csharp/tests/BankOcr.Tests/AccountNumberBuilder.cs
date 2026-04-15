using System.Text;

namespace BankOcr.Tests;

/// <summary>
/// Builds OCR 3x27 input for a 9-digit account number. <see cref="FromString"/> is the ergonomic entry point;
/// <see cref="WithDigit"/> lets a test overwrite one position (e.g. to inject a malformed glyph for ILL scenarios).
/// </summary>
public class AccountNumberBuilder
{
    private readonly string[][] _digits = new string[OcrDimensions.AccountLength][];

    public AccountNumberBuilder()
    {
        for (var i = 0; i < OcrDimensions.AccountLength; i++)
            _digits[i] = new DigitBuilder().ForDigit(0).Build();
    }

    public AccountNumberBuilder FromString(string digits)
    {
        if (digits.Length != OcrDimensions.AccountLength)
            throw new ArgumentException(
                $"Expected {OcrDimensions.AccountLength} digits, got {digits.Length}.", nameof(digits));
        for (var i = 0; i < OcrDimensions.AccountLength; i++)
            _digits[i] = new DigitBuilder().ForDigit(digits[i] - '0').Build();
        return this;
    }

    public AccountNumberBuilder WithDigitAt(int position, string[] glyph)
    {
        _digits[position] = (string[])glyph.Clone();
        return this;
    }

    public string[] BuildRows()
    {
        var rows = new string[OcrDimensions.DigitHeight];
        for (var r = 0; r < OcrDimensions.DigitHeight; r++)
        {
            var sb = new StringBuilder(OcrDimensions.RowWidth);
            for (var d = 0; d < OcrDimensions.AccountLength; d++)
                sb.Append(_digits[d][r]);
            rows[r] = sb.ToString();
        }
        return rows;
    }
}
