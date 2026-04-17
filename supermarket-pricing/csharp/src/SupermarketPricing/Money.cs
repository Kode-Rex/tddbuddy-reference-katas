namespace SupermarketPricing;

/// <summary>
/// Monetary amount in cents. Using integer cents avoids floating-point rounding
/// errors that plague price calculations. All arithmetic stays in whole cents;
/// conversions to display dollars happen at the boundary, not inside the domain.
/// </summary>
public readonly record struct Money(int Cents)
{
    public static Money Zero => new(0);
    public static Money operator +(Money a, Money b) => new(a.Cents + b.Cents);
    public static Money operator -(Money a, Money b) => new(a.Cents - b.Cents);
    public static Money operator *(Money a, int factor) => new(a.Cents * factor);
}
