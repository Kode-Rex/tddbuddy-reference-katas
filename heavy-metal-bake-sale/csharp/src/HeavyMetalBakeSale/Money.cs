namespace HeavyMetalBakeSale;

public readonly record struct Money(decimal Amount)
{
    public static Money Zero => new(0m);

    public bool IsNonNegative => Amount >= 0m;

    public static Money operator +(Money a, Money b) => new(a.Amount + b.Amount);
    public static Money operator -(Money a, Money b) => new(a.Amount - b.Amount);
    public static bool operator >(Money a, Money b) => a.Amount > b.Amount;
    public static bool operator <(Money a, Money b) => a.Amount < b.Amount;
    public static bool operator >=(Money a, Money b) => a.Amount >= b.Amount;
    public static bool operator <=(Money a, Money b) => a.Amount <= b.Amount;

    public string ToDisplay() => $"${Amount:F2}";
}
