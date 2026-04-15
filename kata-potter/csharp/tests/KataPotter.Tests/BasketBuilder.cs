using static KataPotter.PricingRules;

namespace KataPotter.Tests;

// Test-folder fluent synthesiser. Place copies of books into the basket by id.
// Reads as the basket literal under test: "two of book 1, one of book 2" etc.
public class BasketBuilder
{
    private readonly int[] _counts = new int[MaxBookId + 1];

    public BasketBuilder WithBook(int series, int count)
    {
        if (series < MinBookId || series > MaxBookId)
            throw new BookOutOfRangeException();
        if (count < 0) count = 0;
        _counts[series] += count;
        return this;
    }

    public Basket Build() => new((int[])_counts.Clone());
}
