namespace ShoppingCart;

public interface IDiscountPolicy
{
    Money Apply(Money unitPrice, Quantity quantity);
}

public sealed class NoDiscount : IDiscountPolicy
{
    public static readonly NoDiscount Instance = new();

    public Money Apply(Money unitPrice, Quantity quantity) => unitPrice * quantity.Value;
}
