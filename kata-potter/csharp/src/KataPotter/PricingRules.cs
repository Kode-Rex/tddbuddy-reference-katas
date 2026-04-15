namespace KataPotter;

// Identical byte-for-byte across C#, TypeScript, and Python.
// The exception messages are the spec (see ../SCENARIOS.md).
public static class BasketMessages
{
    public const string BookOutOfRange = "book id must be between 1 and 5";
}

public static class PricingRules
{
    public const decimal BasePrice = 8.00m;

    public const int MinBookId = 1;
    public const int MaxBookId = 5;

    // Discount table indexed by set size. Index 0 is unused; index 1..5 are
    // the discount multipliers (1 - discount) applied to `k * BasePrice`.
    public static readonly decimal[] SetDiscount =
    {
        0.00m, // index 0 — unused
        0.00m, // 1 book  — no discount
        0.05m, // 2 books — 5%
        0.10m, // 3 books — 10%
        0.20m, // 4 books — 20%
        0.25m, // 5 books — 25%
    };

    public static decimal PriceOfSet(int distinctBooks)
    {
        var discount = SetDiscount[distinctBooks];
        return distinctBooks * BasePrice * (1m - discount);
    }
}
