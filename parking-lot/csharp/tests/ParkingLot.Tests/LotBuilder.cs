namespace ParkingLot.Tests;

public class LotBuilder
{
    private int _motorcycleSpots;
    private int _compactSpots;
    private int _largeSpots;
    private decimal _motorcycleRate = Lot.DefaultMotorcycleRate;
    private decimal _carRate = Lot.DefaultCarRate;
    private decimal _busRate = Lot.DefaultBusRate;
    private DateTime _start = new(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    private FixedClock? _clock;

    public LotBuilder WithMotorcycleSpots(int count) { _motorcycleSpots = count; return this; }

    public LotBuilder WithCompactSpots(int count) { _compactSpots = count; return this; }

    public LotBuilder WithLargeSpots(int count) { _largeSpots = count; return this; }

    public LotBuilder WithMotorcycleRate(decimal rate) { _motorcycleRate = rate; return this; }

    public LotBuilder WithCarRate(decimal rate) { _carRate = rate; return this; }

    public LotBuilder WithBusRate(decimal rate) { _busRate = rate; return this; }

    public LotBuilder StartingAt(DateTime start) { _start = start; return this; }

    public LotBuilder WithClock(FixedClock clock) { _clock = clock; return this; }

    public (Lot Lot, FixedClock Clock) Build()
    {
        var clock = _clock ?? new FixedClock(_start);
        var lot = new Lot(_motorcycleSpots, _compactSpots, _largeSpots, clock,
            _motorcycleRate, _carRate, _busRate);
        return (lot, clock);
    }
}
