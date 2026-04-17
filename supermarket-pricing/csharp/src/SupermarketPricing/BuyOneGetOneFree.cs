namespace SupermarketPricing;

/// <summary>
/// Every second item is free. For N items, ceil(N/2) are charged at unit price.
/// </summary>
public sealed class BuyOneGetOneFree : IPricingRule
{
    public Money ItemPrice { get; }

    public BuyOneGetOneFree(Money itemPrice)
    {
        ItemPrice = itemPrice;
    }

    public Money Calculate(int quantity, Weight weight)
    {
        var chargeable = (quantity + 1) / 2;
        return ItemPrice * chargeable;
    }
}
