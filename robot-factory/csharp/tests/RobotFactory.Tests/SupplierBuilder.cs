namespace RobotFactory.Tests;

public class SupplierBuilder
{
    private string _name = "Supplier";
    private readonly List<(PartType Type, PartOption Option, Money Price)> _parts = new();

    public SupplierBuilder Named(string name) { _name = name; return this; }

    public SupplierBuilder WithPart(PartType type, PartOption option, decimal price)
    {
        _parts.Add((type, option, new Money(price)));
        return this;
    }

    public FakePartSupplier Build()
    {
        var supplier = new FakePartSupplier(_name);
        foreach (var (type, option, price) in _parts)
        {
            supplier.WithPart(type, option, price);
        }
        return supplier;
    }
}
