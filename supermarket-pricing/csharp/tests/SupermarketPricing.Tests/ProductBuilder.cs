namespace SupermarketPricing.Tests;

public class ProductBuilder
{
    private string _sku = "X";
    private string _name = "Default Item";
    private IPricingRule _pricingRule = new UnitPrice(new Money(100));

    public ProductBuilder WithSku(string sku) { _sku = sku; return this; }
    public ProductBuilder Named(string name) { _name = name; return this; }
    public ProductBuilder WithUnitPrice(int cents) { _pricingRule = new UnitPrice(new Money(cents)); return this; }
    public ProductBuilder WithMultiBuy(int groupSize, int groupCents, int itemCents)
    {
        _pricingRule = new MultiBuy(groupSize, new Money(groupCents), new Money(itemCents));
        return this;
    }
    public ProductBuilder WithBuyOneGetOneFree(int itemCents)
    {
        _pricingRule = new BuyOneGetOneFree(new Money(itemCents));
        return this;
    }
    public ProductBuilder WithWeightedPrice(int centsPerKg)
    {
        _pricingRule = new WeightedPrice(centsPerKg);
        return this;
    }
    public ProductBuilder WithPricingRule(IPricingRule rule) { _pricingRule = rule; return this; }

    public Product Build() => new(_sku, _name, _pricingRule);
}
