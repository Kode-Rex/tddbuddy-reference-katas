namespace WeatherStation;

public class Station
{
    private readonly IClock _clock;
    private readonly AlertThresholds _thresholds;
    private readonly List<Reading> _readings = new();

    public Station(IClock clock, AlertThresholds thresholds = default)
    {
        _clock = clock;
        _thresholds = thresholds;
    }

    public IReadOnlyList<Reading> Readings => _readings;

    public IReadOnlyList<Alert> Record(decimal temperature, decimal humidity, decimal windSpeed)
    {
        if (humidity < 0m || humidity > 100m)
            throw new InvalidReadingException("Humidity must be between 0 and 100.");
        if (windSpeed < 0m)
            throw new InvalidReadingException("Wind speed must be non-negative.");

        var reading = new Reading(temperature, humidity, windSpeed, _clock.Now());
        _readings.Add(reading);

        return EvaluateAlerts(reading);
    }

    public Statistics GetStatistics()
    {
        if (_readings.Count == 0)
            throw new NoReadingsException();

        return new Statistics(
            MinTemperature: _readings.Min(r => r.Temperature),
            MaxTemperature: _readings.Max(r => r.Temperature),
            AvgTemperature: _readings.Average(r => r.Temperature),
            MinHumidity: _readings.Min(r => r.Humidity),
            MaxHumidity: _readings.Max(r => r.Humidity),
            AvgHumidity: _readings.Average(r => r.Humidity),
            MaxWindSpeed: _readings.Max(r => r.WindSpeed),
            AvgWindSpeed: _readings.Average(r => r.WindSpeed));
    }

    private List<Alert> EvaluateAlerts(Reading reading)
    {
        var alerts = new List<Alert>();

        if (_thresholds.HighTemperatureCeiling.HasValue &&
            reading.Temperature > _thresholds.HighTemperatureCeiling.Value)
        {
            alerts.Add(new Alert($"High temperature alert: {reading.Temperature} exceeds ceiling of {_thresholds.HighTemperatureCeiling.Value}."));
        }

        if (_thresholds.LowTemperatureFloor.HasValue &&
            reading.Temperature < _thresholds.LowTemperatureFloor.Value)
        {
            alerts.Add(new Alert($"Low temperature alert: {reading.Temperature} is below floor of {_thresholds.LowTemperatureFloor.Value}."));
        }

        if (_thresholds.HighWindSpeedLimit.HasValue &&
            reading.WindSpeed > _thresholds.HighWindSpeedLimit.Value)
        {
            alerts.Add(new Alert($"High wind alert: {reading.WindSpeed} exceeds limit of {_thresholds.HighWindSpeedLimit.Value}."));
        }

        return alerts;
    }
}
