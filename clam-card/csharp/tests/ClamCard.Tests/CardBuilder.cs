namespace ClamCard.Tests;

// Test-folder synthesiser. Stages a card with a known set of zone-tagged
// stations. `OnDay` is accepted for scenario readability; caps reset at
// card construction in this reference, so the date is effectively "today".
public class CardBuilder
{
    private readonly Dictionary<string, Zone> _stations = new();

    public CardBuilder OnDay(DateOnly _) => this;

    public CardBuilder WithZone(Zone zone, params string[] stations)
    {
        foreach (var s in stations) _stations[s] = zone;
        return this;
    }

    public Card Build() => new(_stations);
}
