namespace ShoppingCart;

public sealed class BulkPricing : IDiscountPolicy
{
    public Quantity Threshold { get; }
    public Money BulkUnitPrice { get; }

    public BulkPricing(Quantity threshold, Money bulkUnitPrice)
    {
        if (bulkUnitPrice < Money.Zero)
        {
            throw new ArgumentOutOfRangeException(nameof(bulkUnitPrice), "Bulk unit price must not be negative");
        }
        Threshold = threshold;
        BulkUnitPrice = bulkUnitPrice;
    }

    public Money Apply(Money unitPrice, Quantity quantity)
    {
        var effectiveUnitPrice = quantity.Value >= Threshold.Value ? BulkUnitPrice : unitPrice;
        return effectiveUnitPrice * quantity.Value;
    }
}
