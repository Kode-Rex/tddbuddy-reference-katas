namespace HeavyMetalBakeSale;

public class InsufficientPaymentException : Exception
{
    public InsufficientPaymentException()
        : base("Not enough money.")
    {
    }
}
