namespace VideoClubRental;

public readonly record struct Age(int Years)
{
    public const int AdultMinimum = 18;

    public bool IsAdult => Years >= AdultMinimum;
}
