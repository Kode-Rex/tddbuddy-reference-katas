namespace SupermarketPricing;

/// <summary>
/// A combo deal that applies when both SKUs are present in the checkout.
/// Each qualifying pair of (skuA, skuB) is charged at the deal price instead
/// of the sum of their individual prices. Applies once per qualifying pair.
/// </summary>
public sealed class ComboDeal
{
    public string SkuA { get; }
    public string SkuB { get; }
    public Money DealPrice { get; }

    public ComboDeal(string skuA, string skuB, Money dealPrice)
    {
        SkuA = skuA;
        SkuB = skuB;
        DealPrice = dealPrice;
    }
}
