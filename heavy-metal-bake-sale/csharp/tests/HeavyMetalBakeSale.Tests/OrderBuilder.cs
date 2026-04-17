namespace HeavyMetalBakeSale.Tests;

public class OrderBuilder
{
    private readonly List<Product> _inventory = new();

    public OrderBuilder WithProduct(Product product)
    {
        _inventory.Add(product);
        return this;
    }

    public OrderBuilder WithDefaultInventory()
    {
        _inventory.Add(new Product("Brownie", new Money(0.75m), "B", 48));
        _inventory.Add(new Product("Muffin", new Money(1.00m), "M", 36));
        _inventory.Add(new Product("Cake Pop", new Money(1.35m), "C", 24));
        _inventory.Add(new Product("Water", new Money(1.50m), "W", 30));
        return this;
    }

    public BakeSale Build() => new(_inventory);
}
