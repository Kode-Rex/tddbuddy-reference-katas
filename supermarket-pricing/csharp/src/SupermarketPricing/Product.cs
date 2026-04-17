namespace SupermarketPricing;

/// <summary>
/// A catalog entry: SKU, name, and pricing rule. The pricing rule is a strategy
/// that determines how the product's charge is computed based on quantity or weight.
/// </summary>
public sealed class Product
{
    public string Sku { get; }
    public string Name { get; }
    public IPricingRule PricingRule { get; }

    public Product(string sku, string name, IPricingRule pricingRule)
    {
        if (string.IsNullOrWhiteSpace(sku))
        {
            throw new ArgumentException("SKU must not be empty", nameof(sku));
        }
        Sku = sku;
        Name = name;
        PricingRule = pricingRule;
    }
}
