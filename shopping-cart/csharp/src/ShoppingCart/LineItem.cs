namespace ShoppingCart;

public sealed class LineItem
{
    public Product Product { get; }
    public Quantity Quantity { get; private set; }

    public LineItem(Product product, Quantity quantity)
    {
        Product = product;
        Quantity = quantity;
    }

    public Money Subtotal() => Product.DiscountPolicy.Apply(Product.UnitPrice, Quantity);

    internal void IncrementBy(Quantity delta)
    {
        Quantity = new Quantity(Quantity.Value + delta.Value);
    }

    internal void SetQuantity(Quantity quantity)
    {
        Quantity = quantity;
    }
}
