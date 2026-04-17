namespace HeavyMetalBakeSale;

public class OutOfStockException : Exception
{
    public OutOfStockException(string productName)
        : base($"{productName} is out of stock")
    {
    }
}
