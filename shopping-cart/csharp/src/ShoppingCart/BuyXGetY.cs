namespace ShoppingCart;

public sealed class BuyXGetY : IDiscountPolicy
{
    public int BuyCount { get; }
    public int FreeCount { get; }

    public BuyXGetY(int buyCount, int freeCount)
    {
        if (buyCount <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(buyCount), "Buy count must be positive");
        }
        if (freeCount <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(freeCount), "Free count must be positive");
        }
        BuyCount = buyCount;
        FreeCount = freeCount;
    }

    public Money Apply(Money unitPrice, Quantity quantity)
    {
        var groupSize = BuyCount + FreeCount;
        var fullGroups = quantity.Value / groupSize;
        var remainder = quantity.Value % groupSize;
        var chargeable = (fullGroups * BuyCount) + Math.Min(remainder, BuyCount);
        return unitPrice * chargeable;
    }
}
