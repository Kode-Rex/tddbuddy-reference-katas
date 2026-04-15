namespace ShoppingCart;

public sealed class FixedOff : IDiscountPolicy
{
    public Money Amount { get; }

    public FixedOff(Money amount)
    {
        if (amount < Money.Zero)
        {
            throw new ArgumentOutOfRangeException(nameof(amount), "Fixed discount amount must not be negative");
        }
        Amount = amount;
    }

    public Money Apply(Money unitPrice, Quantity quantity)
    {
        var gross = unitPrice * quantity.Value;
        var discounted = gross - Amount;
        return discounted < Money.Zero ? Money.Zero : discounted;
    }
}
