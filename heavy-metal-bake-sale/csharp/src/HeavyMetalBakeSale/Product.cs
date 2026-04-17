namespace HeavyMetalBakeSale;

public class Product
{
    public string Name { get; }
    public Money Price { get; }
    public string PurchaseCode { get; }
    public int Stock { get; private set; }

    public Product(string name, Money price, string purchaseCode, int stock)
    {
        Name = name;
        Price = price;
        PurchaseCode = purchaseCode;
        Stock = stock;
    }

    public bool IsInStock => Stock > 0;

    public void DecrementStock()
    {
        Stock--;
    }
}
