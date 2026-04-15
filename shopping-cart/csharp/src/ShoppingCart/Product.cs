namespace ShoppingCart;

public sealed class Product
{
    public string Sku { get; }
    public string Name { get; }
    public Money UnitPrice { get; }
    public IDiscountPolicy DiscountPolicy { get; }

    public Product(string sku, string name, Money unitPrice, IDiscountPolicy? discountPolicy = null)
    {
        if (string.IsNullOrWhiteSpace(sku))
        {
            throw new ArgumentException("SKU must not be empty", nameof(sku));
        }
        Sku = sku;
        Name = name;
        UnitPrice = unitPrice;
        DiscountPolicy = discountPolicy ?? NoDiscount.Instance;
    }
}
