namespace SupermarketPricing;

/// <summary>
/// Buy N items for a special group price; remaining items at unit price.
/// Example: 3 for $1.30 means every group of 3 costs 130 cents, leftover items at 50 cents each.
/// </summary>
public sealed class MultiBuy : IPricingRule
{
    public int GroupSize { get; }
    public Money GroupPrice { get; }
    public Money ItemPrice { get; }

    public MultiBuy(int groupSize, Money groupPrice, Money itemPrice)
    {
        if (groupSize <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(groupSize), "Group size must be positive");
        }
        GroupSize = groupSize;
        GroupPrice = groupPrice;
        ItemPrice = itemPrice;
    }

    public Money Calculate(int quantity, Weight weight)
    {
        var fullGroups = quantity / GroupSize;
        var remainder = quantity % GroupSize;
        return GroupPrice * fullGroups + ItemPrice * remainder;
    }
}
