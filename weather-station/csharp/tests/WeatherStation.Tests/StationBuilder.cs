namespace WeatherStation.Tests;

public class StationBuilder
{
    private DateTime _startTime = new(2026, 6, 15, 12, 0, 0, DateTimeKind.Utc);
    private AlertThresholds _thresholds = default;
    private readonly List<(DateTime Time, decimal Temperature, decimal Humidity, decimal WindSpeed)> _seededReadings = new();

    public StationBuilder StartingAt(DateTime time) { _startTime = time; return this; }

    public StationBuilder WithThresholds(AlertThresholds thresholds) { _thresholds = thresholds; return this; }

    public StationBuilder WithReading(decimal temperature, decimal humidity, decimal windSpeed, DateTime? at = null)
    {
        _seededReadings.Add((at ?? _startTime, temperature, humidity, windSpeed));
        return this;
    }

    public (Station Station, FixedClock Clock) Build()
    {
        var clock = new FixedClock(_startTime);
        var station = new Station(clock, _thresholds);
        foreach (var (time, temp, hum, wind) in _seededReadings)
        {
            clock.AdvanceTo(time);
            station.Record(temp, hum, wind);
        }
        return (station, clock);
    }
}
