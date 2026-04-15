namespace ShoppingCart.Tests;

public class ProductBuilder
{
    private string _sku = "SKU-DEFAULT";
    private string _name = "Widget";
    private Money _unitPrice = new(10m);
    private IDiscountPolicy? _discountPolicy;

    public ProductBuilder WithSku(string sku) { _sku = sku; return this; }
    public ProductBuilder Named(string name) { _name = name; return this; }
    public ProductBuilder PricedAt(decimal amount) { _unitPrice = new Money(amount); return this; }
    public ProductBuilder WithDiscount(IDiscountPolicy policy) { _discountPolicy = policy; return this; }

    public Product Build() => new(_sku, _name, _unitPrice, _discountPolicy);
}
