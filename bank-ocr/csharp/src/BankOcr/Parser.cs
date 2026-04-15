namespace BankOcr;

public static class Parser
{
    // Canonical glyphs: each entry is the 9-char flattened 3x3 grid (top | middle | bottom).
    private static readonly IReadOnlyDictionary<string, int> Glyphs = new Dictionary<string, int>
    {
        [" _ " + "| |" + "|_|"] = 0,
        ["   " + "  |" + "  |"] = 1,
        [" _ " + " _|" + "|_ "] = 2,
        [" _ " + " _|" + " _|"] = 3,
        ["   " + "|_|" + "  |"] = 4,
        [" _ " + "|_ " + " _|"] = 5,
        [" _ " + "|_ " + "|_|"] = 6,
        [" _ " + "  |" + "  |"] = 7,
        [" _ " + "|_|" + "|_|"] = 8,
        [" _ " + "|_|" + " _|"] = 9,
    };

    public static Digit ParseDigit(IReadOnlyList<string> threeRowBlock)
    {
        if (threeRowBlock.Count != OcrDimensions.DigitHeight)
            throw new InvalidAccountNumberFormatException(
                $"Digit block must have exactly {OcrDimensions.DigitHeight} rows.");
        foreach (var row in threeRowBlock)
        {
            if (row.Length != OcrDimensions.DigitWidth)
                throw new InvalidAccountNumberFormatException(
                    $"Digit block rows must be exactly {OcrDimensions.DigitWidth} characters wide.");
        }
        var key = threeRowBlock[0] + threeRowBlock[1] + threeRowBlock[2];
        return Glyphs.TryGetValue(key, out var value) ? Digit.Of(value) : Digit.Unknown;
    }

    public static AccountNumber ParseAccountNumber(IReadOnlyList<string> threeRows)
    {
        if (threeRows.Count != OcrDimensions.DigitHeight)
            throw new InvalidAccountNumberFormatException(
                $"OCR block must have exactly {OcrDimensions.DigitHeight} rows.");
        foreach (var row in threeRows)
        {
            if (row.Length != OcrDimensions.RowWidth)
                throw new InvalidAccountNumberFormatException(
                    $"OCR block rows must be exactly {OcrDimensions.RowWidth} characters wide.");
        }

        var digits = new List<Digit>(OcrDimensions.AccountLength);
        for (var i = 0; i < OcrDimensions.AccountLength; i++)
        {
            var start = i * OcrDimensions.DigitWidth;
            var block = new[]
            {
                threeRows[0].Substring(start, OcrDimensions.DigitWidth),
                threeRows[1].Substring(start, OcrDimensions.DigitWidth),
                threeRows[2].Substring(start, OcrDimensions.DigitWidth),
            };
            digits.Add(ParseDigit(block));
        }
        return new AccountNumber(digits);
    }
}
