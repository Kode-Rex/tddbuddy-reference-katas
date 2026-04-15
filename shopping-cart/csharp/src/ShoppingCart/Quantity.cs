namespace ShoppingCart;

public readonly record struct Quantity
{
    public int Value { get; }

    public Quantity(int value)
    {
        if (value <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(value), "Quantity must be a positive whole number");
        }
        Value = value;
    }

    public static implicit operator int(Quantity q) => q.Value;

    public override string ToString() => Value.ToString();
}
