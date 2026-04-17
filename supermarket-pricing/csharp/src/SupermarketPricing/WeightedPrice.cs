namespace SupermarketPricing;

/// <summary>
/// Price per kilogram. Charge = weight × cents-per-kg, rounded to nearest cent.
/// </summary>
public sealed class WeightedPrice : IPricingRule
{
    public int CentsPerKg { get; }

    public WeightedPrice(int centsPerKg)
    {
        if (centsPerKg < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(centsPerKg), "Price per kg must not be negative");
        }
        CentsPerKg = centsPerKg;
    }

    public Money Calculate(int quantity, Weight weight)
    {
        var rawCents = weight.Kg * CentsPerKg;
        var rounded = (int)Math.Round(rawCents, MidpointRounding.AwayFromZero);
        return new Money(rounded);
    }
}
