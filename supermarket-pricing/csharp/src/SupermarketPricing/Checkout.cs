namespace SupermarketPricing;

/// <summary>
/// The checkout aggregate. Accumulates scanned items (by count or weight) and
/// computes the total by delegating to each product's pricing rule, then
/// applying any combo deals.
/// </summary>
public sealed class Checkout
{
    private readonly Dictionary<string, Product> _products = new();
    private readonly Dictionary<string, int> _quantities = new();
    private readonly Dictionary<string, Weight> _weights = new();
    private readonly List<ComboDeal> _comboDeals = new();

    public Checkout(IEnumerable<ComboDeal>? comboDeals = null)
    {
        if (comboDeals is not null)
        {
            _comboDeals.AddRange(comboDeals);
        }
    }

    public void Scan(Product product)
    {
        EnsureRegistered(product);
        _quantities[product.Sku] = _quantities.GetValueOrDefault(product.Sku) + 1;
    }

    public void ScanWeighted(Product product, Weight weight)
    {
        EnsureRegistered(product);
        var existing = _weights.GetValueOrDefault(product.Sku);
        _weights[product.Sku] = new Weight(existing.Kg + weight.Kg);
    }

    public Money Total()
    {
        var total = Money.Zero;

        // Track how many items are consumed by combo deals
        var comboConsumed = new Dictionary<string, int>();

        foreach (var deal in _comboDeals)
        {
            var countA = _quantities.GetValueOrDefault(deal.SkuA) - comboConsumed.GetValueOrDefault(deal.SkuA);
            var countB = _quantities.GetValueOrDefault(deal.SkuB) - comboConsumed.GetValueOrDefault(deal.SkuB);
            var pairs = Math.Min(countA, countB);

            if (pairs > 0)
            {
                total += deal.DealPrice * pairs;
                comboConsumed[deal.SkuA] = comboConsumed.GetValueOrDefault(deal.SkuA) + pairs;
                comboConsumed[deal.SkuB] = comboConsumed.GetValueOrDefault(deal.SkuB) + pairs;
            }
        }

        foreach (var (sku, product) in _products)
        {
            var quantity = _quantities.GetValueOrDefault(sku) - comboConsumed.GetValueOrDefault(sku);
            var weight = _weights.GetValueOrDefault(sku);

            if (quantity > 0 || weight.Kg > 0)
            {
                total += product.PricingRule.Calculate(quantity, weight);
            }
        }

        return total;
    }

    private void EnsureRegistered(Product product)
    {
        _products.TryAdd(product.Sku, product);
    }
}
