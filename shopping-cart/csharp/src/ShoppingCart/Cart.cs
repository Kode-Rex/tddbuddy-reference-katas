namespace ShoppingCart;

public sealed class Cart
{
    private static readonly Quantity One = new(1);

    private readonly List<LineItem> _lines = new();

    public IReadOnlyList<LineItem> Lines => _lines;

    public bool IsEmpty => _lines.Count == 0;

    public void Add(Product product) => Add(product, One);

    public void Add(Product product, Quantity quantity)
    {
        var existing = FindLine(product.Sku);
        if (existing is null)
        {
            _lines.Add(new LineItem(product, quantity));
        }
        else
        {
            existing.IncrementBy(quantity);
        }
    }

    public void Remove(string sku)
    {
        var existing = FindLine(sku);
        if (existing is not null)
        {
            _lines.Remove(existing);
        }
    }

    public void UpdateQuantity(string sku, int quantity)
    {
        var line = FindLine(sku)
            ?? throw new InvalidOperationException($"No line item for SKU '{sku}'");
        line.SetQuantity(new Quantity(quantity));
    }

    public Money Total()
    {
        var total = Money.Zero;
        foreach (var line in _lines)
        {
            total += line.Subtotal();
        }
        return total;
    }

    private LineItem? FindLine(string sku) => _lines.FirstOrDefault(l => l.Product.Sku == sku);
}
