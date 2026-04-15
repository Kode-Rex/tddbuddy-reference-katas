namespace BankOcr;

public readonly record struct Digit
{
    public int? Value { get; }

    private Digit(int? value) { Value = value; }

    public static Digit Of(int value)
    {
        if (value < 0 || value > 9)
            throw new ArgumentOutOfRangeException(nameof(value), "Digit value must be 0-9.");
        return new Digit(value);
    }

    public static Digit Unknown { get; } = new(null);

    public bool IsKnown => Value.HasValue;

    public override string ToString() => IsKnown ? Value!.Value.ToString() : "?";
}
