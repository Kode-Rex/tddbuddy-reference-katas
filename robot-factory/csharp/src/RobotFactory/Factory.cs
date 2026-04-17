namespace RobotFactory;

public class Factory
{
    private readonly List<IPartSupplier> _suppliers;

    public Factory(IEnumerable<IPartSupplier> suppliers)
    {
        _suppliers = suppliers.ToList();
    }

    public CostBreakdown CostRobot(RobotOrder order)
    {
        order.Validate();

        var cheapestParts = new List<PartQuote>();

        foreach (var (type, option) in order.Parts)
        {
            var quotes = _suppliers
                .Select(s => s.GetQuote(type, option))
                .Where(q => q is not null)
                .Cast<PartQuote>()
                .ToList();

            if (quotes.Count == 0)
            {
                throw new PartNotAvailableException($"Part not available: {option}");
            }

            cheapestParts.Add(quotes.MinBy(q => q.Price)!);
        }

        var total = cheapestParts.Aggregate(Money.Zero, (sum, q) => sum + q.Price);
        return new CostBreakdown(cheapestParts, total);
    }

    public IReadOnlyList<PurchasedPart> PurchaseRobot(RobotOrder order)
    {
        var breakdown = CostRobot(order);

        var purchased = new List<PurchasedPart>();
        foreach (var quote in breakdown.Parts)
        {
            var supplier = _suppliers.First(s => s.Name == quote.SupplierName);
            purchased.Add(supplier.Purchase(quote.Type, quote.Option));
        }

        return purchased;
    }
}
