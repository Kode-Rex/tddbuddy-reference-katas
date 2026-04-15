namespace VideoClubRental;

public static class PricingPolicy
{
    public static readonly Money BasePrice = new(2.50m);
    public static readonly Money SecondPrice = new(2.25m);
    public static readonly Money ThirdPrice = new(1.75m);

    /// <summary>
    /// Tallies the tiered cost of <paramref name="newRentalCount"/> additional rentals,
    /// given the user already holds <paramref name="existingRentalCount"/> simultaneous rentals.
    /// Slots beyond the third tier continue at the third-tier price.
    /// </summary>
    public static Money Calculate(int newRentalCount, int existingRentalCount)
    {
        var total = Money.Zero;
        for (var i = 0; i < newRentalCount; i++)
        {
            var ordinal = existingRentalCount + i + 1;
            total += PriceForOrdinal(ordinal);
        }
        return total;
    }

    private static Money PriceForOrdinal(int ordinal) => ordinal switch
    {
        1 => BasePrice,
        2 => SecondPrice,
        _ => ThirdPrice,
    };
}
