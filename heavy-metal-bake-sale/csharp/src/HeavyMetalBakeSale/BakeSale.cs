namespace HeavyMetalBakeSale;

public class BakeSale
{
    private readonly List<Product> _inventory;

    public BakeSale(List<Product> inventory)
    {
        _inventory = inventory;
    }

    public IReadOnlyList<Product> Inventory => _inventory;

    public static BakeSale CreateDefault()
    {
        return new BakeSale(new List<Product>
        {
            new("Brownie", new Money(0.75m), "B", 48),
            new("Muffin", new Money(1.00m), "M", 36),
            new("Cake Pop", new Money(1.35m), "C", 24),
            new("Water", new Money(1.50m), "W", 30),
        });
    }

    public Money CalculateTotal(string order)
    {
        var codes = order.Split(',').Select(c => c.Trim()).ToList();
        var products = ResolveProducts(codes);

        ValidateStock(products);

        var total = Money.Zero;
        foreach (var product in products)
        {
            total += product.Price;
        }

        foreach (var product in products)
        {
            product.DecrementStock();
        }

        return total;
    }

    public Money CalculateChange(Money total, Money payment)
    {
        if (payment < total)
        {
            throw new InsufficientPaymentException();
        }

        return payment - total;
    }

    private List<Product> ResolveProducts(List<string> codes)
    {
        var products = new List<Product>();
        foreach (var code in codes)
        {
            var product = _inventory.FirstOrDefault(p => p.PurchaseCode == code);
            if (product is null)
            {
                throw new UnknownPurchaseCodeException(code);
            }
            products.Add(product);
        }
        return products;
    }

    private static void ValidateStock(List<Product> products)
    {
        var grouped = products.GroupBy(p => p.PurchaseCode);
        foreach (var group in grouped)
        {
            var product = group.First();
            if (product.Stock < group.Count())
            {
                throw new OutOfStockException(product.Name);
            }
        }
    }
}
