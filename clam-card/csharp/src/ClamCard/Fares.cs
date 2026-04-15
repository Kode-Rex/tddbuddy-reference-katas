namespace ClamCard;

// Identical dollar amounts across C#, TypeScript, and Python.
// See ../SCENARIOS.md § Domain Rules.
public static class Fares
{
    public const decimal ZoneASingleFare = 2.50m;
    public const decimal ZoneBSingleFare = 3.00m;
    public const decimal ZoneADailyCap = 7.00m;
    public const decimal ZoneBDailyCap = 8.00m;
}

public static class CardMessages
{
    // Identical byte-for-byte across C#, TypeScript, and Python.
    public const string UnknownStation = "station is not on this card's network";
}
