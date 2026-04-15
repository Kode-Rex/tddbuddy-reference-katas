namespace LibraryManagement;

public readonly record struct Money(decimal Amount)
{
    public static Money Zero => new(0m);
    public static Money operator +(Money a, Money b) => new(a.Amount + b.Amount);
    public static Money operator *(Money a, int factor) => new(a.Amount * factor);
    public override string ToString() => $"£{Amount:F2}";
}
