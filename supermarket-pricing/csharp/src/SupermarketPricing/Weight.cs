namespace SupermarketPricing;

/// <summary>
/// A non-negative weight in kilograms. Rejects negative values at construction.
/// </summary>
public readonly record struct Weight
{
    public decimal Kg { get; }

    public Weight(decimal kg)
    {
        if (kg < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(kg), "Weight must not be negative");
        }
        Kg = kg;
    }

    public static Weight Zero => new(0m);
}
