namespace SupermarketPricing.Tests;

public class CheckoutBuilder
{
    private readonly List<(Product Product, int Count)> _scanned = new();
    private readonly List<(Product Product, Weight Weight)> _weighed = new();
    private readonly List<ComboDeal> _comboDeals = new();

    public CheckoutBuilder WithScanned(Product product, int count = 1)
    {
        _scanned.Add((product, count));
        return this;
    }

    public CheckoutBuilder WithWeighed(Product product, decimal kg)
    {
        _weighed.Add((product, new Weight(kg)));
        return this;
    }

    public CheckoutBuilder WithComboDeal(string skuA, string skuB, int dealCents)
    {
        _comboDeals.Add(new ComboDeal(skuA, skuB, new Money(dealCents)));
        return this;
    }

    public Checkout Build()
    {
        var checkout = new Checkout(_comboDeals);
        foreach (var (product, count) in _scanned)
        {
            for (var i = 0; i < count; i++)
            {
                checkout.Scan(product);
            }
        }
        foreach (var (product, weight) in _weighed)
        {
            checkout.ScanWeighted(product, weight);
        }
        return checkout;
    }
}
