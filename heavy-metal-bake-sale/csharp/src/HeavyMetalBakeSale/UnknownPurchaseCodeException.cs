namespace HeavyMetalBakeSale;

public class UnknownPurchaseCodeException : Exception
{
    public UnknownPurchaseCodeException(string code)
        : base($"Unknown purchase code: {code}")
    {
    }
}
