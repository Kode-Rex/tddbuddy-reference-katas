namespace HeavyMetalBakeSale.Tests;

public class ProductBuilder
{
    private string _name = "Brownie";
    private decimal _price = 0.75m;
    private string _purchaseCode = "B";
    private int _stock = 48;

    public ProductBuilder WithName(string name) { _name = name; return this; }
    public ProductBuilder WithPrice(decimal price) { _price = price; return this; }
    public ProductBuilder WithPurchaseCode(string code) { _purchaseCode = code; return this; }
    public ProductBuilder WithStock(int stock) { _stock = stock; return this; }

    public Product Build() => new(_name, new Money(_price), _purchaseCode, _stock);
}
