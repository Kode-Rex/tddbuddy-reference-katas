using FluentAssertions;
using Xunit;

namespace WeatherStation.Tests;

public class StationTests
{
    private static readonly DateTime June15Noon = new(2026, 6, 15, 12, 0, 0, DateTimeKind.Utc);
    private static readonly DateTime June15Pm = new(2026, 6, 15, 14, 0, 0, DateTimeKind.Utc);

    // --- Empty Station ---

    [Fact]
    public void New_station_has_no_readings()
    {
        var (station, _) = new StationBuilder().Build();

        station.Readings.Should().BeEmpty();
    }

    [Fact]
    public void Requesting_statistics_from_a_station_with_no_readings_is_rejected()
    {
        var (station, _) = new StationBuilder().Build();

        var act = () => station.GetStatistics();

        act.Should().Throw<NoReadingsException>()
            .WithMessage("Cannot compute statistics with no readings.");
    }

    // --- Recording Readings ---

    [Fact]
    public void Recording_a_valid_reading_increases_the_reading_count()
    {
        var (station, _) = new StationBuilder().Build();

        station.Record(25m, 60m, 15m);

        station.Readings.Should().HaveCount(1);
    }

    [Fact]
    public void Recording_a_reading_captures_the_clock_timestamp()
    {
        var (station, _) = new StationBuilder().StartingAt(June15Noon).Build();

        station.Record(25m, 60m, 15m);

        station.Readings[0].Timestamp.Should().Be(June15Noon);
    }

    [Fact]
    public void Recording_a_reading_with_humidity_below_zero_is_rejected()
    {
        var (station, _) = new StationBuilder().Build();

        var act = () => station.Record(25m, -1m, 10m);

        act.Should().Throw<InvalidReadingException>()
            .WithMessage("Humidity must be between 0 and 100.");
    }

    [Fact]
    public void Recording_a_reading_with_humidity_above_100_is_rejected()
    {
        var (station, _) = new StationBuilder().Build();

        var act = () => station.Record(25m, 101m, 10m);

        act.Should().Throw<InvalidReadingException>()
            .WithMessage("Humidity must be between 0 and 100.");
    }

    [Fact]
    public void Recording_a_reading_with_negative_wind_speed_is_rejected()
    {
        var (station, _) = new StationBuilder().Build();

        var act = () => station.Record(25m, 50m, -1m);

        act.Should().Throw<InvalidReadingException>()
            .WithMessage("Wind speed must be non-negative.");
    }

    [Fact]
    public void Rejected_reading_leaves_the_station_unchanged()
    {
        var (station, _) = new StationBuilder()
            .WithReading(20m, 50m, 10m)
            .Build();
        var countBefore = station.Readings.Count;

        try { station.Record(25m, -1m, 10m); } catch (InvalidReadingException) { }

        station.Readings.Should().HaveCount(countBefore);
    }

    // --- Temperature Statistics ---

    [Fact]
    public void Minimum_temperature_is_the_lowest_recorded_temperature()
    {
        var (station, _) = new StationBuilder()
            .WithReading(30m, 50m, 10m)
            .WithReading(15m, 55m, 12m)
            .WithReading(25m, 60m, 8m)
            .Build();

        station.GetStatistics().MinTemperature.Should().Be(15m);
    }

    [Fact]
    public void Maximum_temperature_is_the_highest_recorded_temperature()
    {
        var (station, _) = new StationBuilder()
            .WithReading(30m, 50m, 10m)
            .WithReading(15m, 55m, 12m)
            .WithReading(25m, 60m, 8m)
            .Build();

        station.GetStatistics().MaxTemperature.Should().Be(30m);
    }

    [Fact]
    public void Average_temperature_is_the_mean_of_all_recorded_temperatures()
    {
        var (station, _) = new StationBuilder()
            .WithReading(30m, 50m, 10m)
            .WithReading(15m, 55m, 12m)
            .WithReading(25m, 60m, 8m)
            .Build();

        // (30 + 15 + 25) / 3 = 23.333...
        station.GetStatistics().AvgTemperature.Should().BeApproximately(23.333m, 0.01m);
    }

    [Fact]
    public void A_single_reading_makes_min_max_and_average_equal()
    {
        var (station, _) = new StationBuilder()
            .WithReading(22m, 65m, 5m)
            .Build();

        var stats = station.GetStatistics();
        stats.MinTemperature.Should().Be(22m);
        stats.MaxTemperature.Should().Be(22m);
        stats.AvgTemperature.Should().Be(22m);
    }

    // --- Humidity Statistics ---

    [Fact]
    public void Minimum_humidity_is_the_lowest_recorded_humidity()
    {
        var (station, _) = new StationBuilder()
            .WithReading(20m, 30m, 10m)
            .WithReading(22m, 80m, 12m)
            .WithReading(21m, 55m, 8m)
            .Build();

        station.GetStatistics().MinHumidity.Should().Be(30m);
    }

    [Fact]
    public void Maximum_humidity_is_the_highest_recorded_humidity()
    {
        var (station, _) = new StationBuilder()
            .WithReading(20m, 30m, 10m)
            .WithReading(22m, 80m, 12m)
            .WithReading(21m, 55m, 8m)
            .Build();

        station.GetStatistics().MaxHumidity.Should().Be(80m);
    }

    [Fact]
    public void Average_humidity_is_the_mean_of_all_recorded_humidities()
    {
        var (station, _) = new StationBuilder()
            .WithReading(20m, 30m, 10m)
            .WithReading(22m, 80m, 12m)
            .WithReading(21m, 55m, 8m)
            .Build();

        // (30 + 80 + 55) / 3 = 55
        station.GetStatistics().AvgHumidity.Should().Be(55m);
    }

    // --- Wind Statistics ---

    [Fact]
    public void Maximum_wind_speed_is_the_highest_recorded_wind_speed()
    {
        var (station, _) = new StationBuilder()
            .WithReading(20m, 50m, 10m)
            .WithReading(22m, 55m, 45m)
            .WithReading(21m, 60m, 20m)
            .Build();

        station.GetStatistics().MaxWindSpeed.Should().Be(45m);
    }

    [Fact]
    public void Average_wind_speed_is_the_mean_of_all_recorded_wind_speeds()
    {
        var (station, _) = new StationBuilder()
            .WithReading(20m, 50m, 10m)
            .WithReading(22m, 55m, 45m)
            .WithReading(21m, 60m, 20m)
            .Build();

        // (10 + 45 + 20) / 3 = 25
        station.GetStatistics().AvgWindSpeed.Should().Be(25m);
    }

    // --- Alerts ---

    [Fact]
    public void High_temperature_alert_fires_when_temperature_exceeds_the_ceiling()
    {
        var (station, _) = new StationBuilder()
            .WithThresholds(new AlertThresholds(HighTemperatureCeiling: 35m))
            .Build();

        var alerts = station.Record(40m, 50m, 10m);

        alerts.Should().ContainSingle()
            .Which.Message.Should().Be("High temperature alert: 40 exceeds ceiling of 35.");
    }

    [Fact]
    public void No_high_temperature_alert_when_temperature_is_at_or_below_the_ceiling()
    {
        var (station, _) = new StationBuilder()
            .WithThresholds(new AlertThresholds(HighTemperatureCeiling: 35m))
            .Build();

        var alerts = station.Record(35m, 50m, 10m);

        alerts.Should().BeEmpty();
    }

    [Fact]
    public void Low_temperature_alert_fires_when_temperature_drops_below_the_floor()
    {
        var (station, _) = new StationBuilder()
            .WithThresholds(new AlertThresholds(LowTemperatureFloor: 0m))
            .Build();

        var alerts = station.Record(-5m, 50m, 10m);

        alerts.Should().ContainSingle()
            .Which.Message.Should().Be("Low temperature alert: -5 is below floor of 0.");
    }

    [Fact]
    public void No_low_temperature_alert_when_temperature_is_at_or_above_the_floor()
    {
        var (station, _) = new StationBuilder()
            .WithThresholds(new AlertThresholds(LowTemperatureFloor: 0m))
            .Build();

        var alerts = station.Record(0m, 50m, 10m);

        alerts.Should().BeEmpty();
    }

    [Fact]
    public void High_wind_alert_fires_when_wind_speed_exceeds_the_limit()
    {
        var (station, _) = new StationBuilder()
            .WithThresholds(new AlertThresholds(HighWindSpeedLimit: 50m))
            .Build();

        var alerts = station.Record(20m, 50m, 60m);

        alerts.Should().ContainSingle()
            .Which.Message.Should().Be("High wind alert: 60 exceeds limit of 50.");
    }

    [Fact]
    public void No_high_wind_alert_when_wind_speed_is_at_or_below_the_limit()
    {
        var (station, _) = new StationBuilder()
            .WithThresholds(new AlertThresholds(HighWindSpeedLimit: 50m))
            .Build();

        var alerts = station.Record(20m, 50m, 50m);

        alerts.Should().BeEmpty();
    }

    [Fact]
    public void Multiple_alerts_can_fire_for_a_single_reading()
    {
        var (station, _) = new StationBuilder()
            .WithThresholds(new AlertThresholds(
                HighTemperatureCeiling: 35m,
                HighWindSpeedLimit: 50m))
            .Build();

        var alerts = station.Record(40m, 50m, 60m);

        alerts.Should().HaveCount(2);
        alerts.Select(a => a.Message).Should().Contain("High temperature alert: 40 exceeds ceiling of 35.");
        alerts.Select(a => a.Message).Should().Contain("High wind alert: 60 exceeds limit of 50.");
    }
}
