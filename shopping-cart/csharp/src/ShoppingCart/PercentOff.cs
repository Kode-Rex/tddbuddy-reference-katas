namespace ShoppingCart;

public sealed class PercentOff : IDiscountPolicy
{
    private const int MinPercent = 0;
    private const int MaxPercent = 100;
    private const decimal HundredPercent = 100m;

    public int Percent { get; }

    public PercentOff(int percent)
    {
        if (percent < MinPercent || percent > MaxPercent)
        {
            throw new ArgumentOutOfRangeException(nameof(percent), "Percent must be between 0 and 100");
        }
        Percent = percent;
    }

    public Money Apply(Money unitPrice, Quantity quantity)
    {
        var gross = unitPrice.Amount * quantity.Value;
        var multiplier = (HundredPercent - Percent) / HundredPercent;
        return new Money(gross * multiplier);
    }
}
