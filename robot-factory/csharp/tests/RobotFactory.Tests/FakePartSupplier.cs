namespace RobotFactory.Tests;

public class FakePartSupplier : IPartSupplier
{
    private readonly Dictionary<(PartType, PartOption), Money> _catalog = new();
    private readonly List<PurchasedPart> _purchaseLog = new();

    public FakePartSupplier(string name) => Name = name;

    public string Name { get; }

    public IReadOnlyList<PurchasedPart> PurchaseLog => _purchaseLog;

    public FakePartSupplier WithPart(PartType type, PartOption option, Money price)
    {
        _catalog[(type, option)] = price;
        return this;
    }

    public PartQuote? GetQuote(PartType type, PartOption option)
    {
        return _catalog.TryGetValue((type, option), out var price)
            ? new PartQuote(type, option, price, Name)
            : null;
    }

    public PurchasedPart Purchase(PartType type, PartOption option)
    {
        var price = _catalog[(type, option)];
        var part = new PurchasedPart(type, option, price, Name);
        _purchaseLog.Add(part);
        return part;
    }
}
