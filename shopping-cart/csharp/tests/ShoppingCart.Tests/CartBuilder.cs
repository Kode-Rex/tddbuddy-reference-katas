namespace ShoppingCart.Tests;

public class CartBuilder
{
    private readonly List<(Product Product, Quantity Quantity)> _seeded = new();

    public CartBuilder WithProduct(Product product) => WithProduct(product, new Quantity(1));

    public CartBuilder WithProduct(Product product, Quantity quantity)
    {
        _seeded.Add((product, quantity));
        return this;
    }

    public Cart Build()
    {
        var cart = new Cart();
        foreach (var (product, quantity) in _seeded)
        {
            cart.Add(product, quantity);
        }
        return cart;
    }
}
