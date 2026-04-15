namespace ClamCard.Tests;

public class RideBuilder
{
    private string _from = "";
    private string _to = "";
    private Zone _zone = Zone.A;
    private decimal _fare;

    public RideBuilder From(string station) { _from = station; return this; }
    public RideBuilder To(string station) { _to = station; return this; }
    public RideBuilder ChargedAt(Zone zone) { _zone = zone; return this; }
    public RideBuilder WithFare(decimal fare) { _fare = fare; return this; }

    public Ride Build() => new(_from, _to, _zone, _fare);
}
