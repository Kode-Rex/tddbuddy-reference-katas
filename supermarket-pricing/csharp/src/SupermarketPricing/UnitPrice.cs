namespace SupermarketPricing;

/// <summary>
/// Simplest pricing: each item costs a fixed amount.
/// </summary>
public sealed class UnitPrice : IPricingRule
{
    public Money Price { get; }

    public UnitPrice(Money price)
    {
        Price = price;
    }

    public Money Calculate(int quantity, Weight weight) => Price * quantity;
}
