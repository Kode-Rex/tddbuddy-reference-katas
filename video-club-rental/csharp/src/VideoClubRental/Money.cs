namespace VideoClubRental;

public readonly record struct Money(decimal Amount)
{
    public static Money Zero => new(0m);
    public static Money operator +(Money a, Money b) => new(a.Amount + b.Amount);
    public override string ToString() => $"£{Amount:F2}";
}
