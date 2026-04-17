from datetime import datetime, timezone
from decimal import Decimal

import pytest

from weather_station import AlertThresholds, InvalidReadingError, NoReadingsError

from .station_builder import StationBuilder

JUNE_15_NOON = datetime(2026, 6, 15, 12, 0, 0, tzinfo=timezone.utc)


# --- Empty Station ---


def test_new_station_has_no_readings():
    station, _ = StationBuilder().build()
    assert station.readings == []


def test_requesting_statistics_from_a_station_with_no_readings_is_rejected():
    station, _ = StationBuilder().build()
    with pytest.raises(NoReadingsError, match="Cannot compute statistics with no readings."):
        station.get_statistics()


# --- Recording Readings ---


def test_recording_a_valid_reading_increases_the_reading_count():
    station, _ = StationBuilder().build()
    station.record(25, 60, 15)
    assert len(station.readings) == 1


def test_recording_a_reading_captures_the_clock_timestamp():
    station, _ = StationBuilder().starting_at(JUNE_15_NOON).build()
    station.record(25, 60, 15)
    assert station.readings[0].timestamp == JUNE_15_NOON


def test_recording_a_reading_with_humidity_below_zero_is_rejected():
    station, _ = StationBuilder().build()
    with pytest.raises(InvalidReadingError, match="Humidity must be between 0 and 100."):
        station.record(25, -1, 10)


def test_recording_a_reading_with_humidity_above_100_is_rejected():
    station, _ = StationBuilder().build()
    with pytest.raises(InvalidReadingError, match="Humidity must be between 0 and 100."):
        station.record(25, 101, 10)


def test_recording_a_reading_with_negative_wind_speed_is_rejected():
    station, _ = StationBuilder().build()
    with pytest.raises(InvalidReadingError, match="Wind speed must be non-negative."):
        station.record(25, 50, -1)


def test_rejected_reading_leaves_the_station_unchanged():
    station, _ = StationBuilder().with_reading(20, 50, 10).build()
    before = len(station.readings)
    with pytest.raises(InvalidReadingError):
        station.record(25, -1, 10)
    assert len(station.readings) == before


# --- Temperature Statistics ---


def test_minimum_temperature_is_the_lowest_recorded_temperature():
    station, _ = (
        StationBuilder()
        .with_reading(30, 50, 10)
        .with_reading(15, 55, 12)
        .with_reading(25, 60, 8)
        .build()
    )
    assert station.get_statistics().min_temperature == Decimal("15")


def test_maximum_temperature_is_the_highest_recorded_temperature():
    station, _ = (
        StationBuilder()
        .with_reading(30, 50, 10)
        .with_reading(15, 55, 12)
        .with_reading(25, 60, 8)
        .build()
    )
    assert station.get_statistics().max_temperature == Decimal("30")


def test_average_temperature_is_the_mean_of_all_recorded_temperatures():
    station, _ = (
        StationBuilder()
        .with_reading(30, 50, 10)
        .with_reading(15, 55, 12)
        .with_reading(25, 60, 8)
        .build()
    )
    # (30 + 15 + 25) / 3 = 23.333...
    avg = station.get_statistics().avg_temperature
    assert abs(avg - Decimal("23.333")) < Decimal("0.01")


def test_a_single_reading_makes_min_max_and_average_equal():
    station, _ = StationBuilder().with_reading(22, 65, 5).build()
    stats = station.get_statistics()
    assert stats.min_temperature == Decimal("22")
    assert stats.max_temperature == Decimal("22")
    assert stats.avg_temperature == Decimal("22")


# --- Humidity Statistics ---


def test_minimum_humidity_is_the_lowest_recorded_humidity():
    station, _ = (
        StationBuilder()
        .with_reading(20, 30, 10)
        .with_reading(22, 80, 12)
        .with_reading(21, 55, 8)
        .build()
    )
    assert station.get_statistics().min_humidity == Decimal("30")


def test_maximum_humidity_is_the_highest_recorded_humidity():
    station, _ = (
        StationBuilder()
        .with_reading(20, 30, 10)
        .with_reading(22, 80, 12)
        .with_reading(21, 55, 8)
        .build()
    )
    assert station.get_statistics().max_humidity == Decimal("80")


def test_average_humidity_is_the_mean_of_all_recorded_humidities():
    station, _ = (
        StationBuilder()
        .with_reading(20, 30, 10)
        .with_reading(22, 80, 12)
        .with_reading(21, 55, 8)
        .build()
    )
    assert station.get_statistics().avg_humidity == Decimal("55")


# --- Wind Statistics ---


def test_maximum_wind_speed_is_the_highest_recorded_wind_speed():
    station, _ = (
        StationBuilder()
        .with_reading(20, 50, 10)
        .with_reading(22, 55, 45)
        .with_reading(21, 60, 20)
        .build()
    )
    assert station.get_statistics().max_wind_speed == Decimal("45")


def test_average_wind_speed_is_the_mean_of_all_recorded_wind_speeds():
    station, _ = (
        StationBuilder()
        .with_reading(20, 50, 10)
        .with_reading(22, 55, 45)
        .with_reading(21, 60, 20)
        .build()
    )
    assert station.get_statistics().avg_wind_speed == Decimal("25")


# --- Alerts ---


def test_high_temperature_alert_fires_when_temperature_exceeds_the_ceiling():
    station, _ = (
        StationBuilder()
        .with_thresholds(AlertThresholds(high_temperature_ceiling=35))
        .build()
    )
    alerts = station.record(40, 50, 10)
    assert len(alerts) == 1
    assert alerts[0].message == "High temperature alert: 40 exceeds ceiling of 35."


def test_no_high_temperature_alert_when_temperature_is_at_or_below_the_ceiling():
    station, _ = (
        StationBuilder()
        .with_thresholds(AlertThresholds(high_temperature_ceiling=35))
        .build()
    )
    alerts = station.record(35, 50, 10)
    assert len(alerts) == 0


def test_low_temperature_alert_fires_when_temperature_drops_below_the_floor():
    station, _ = (
        StationBuilder()
        .with_thresholds(AlertThresholds(low_temperature_floor=0))
        .build()
    )
    alerts = station.record(-5, 50, 10)
    assert len(alerts) == 1
    assert alerts[0].message == "Low temperature alert: -5 is below floor of 0."


def test_no_low_temperature_alert_when_temperature_is_at_or_above_the_floor():
    station, _ = (
        StationBuilder()
        .with_thresholds(AlertThresholds(low_temperature_floor=0))
        .build()
    )
    alerts = station.record(0, 50, 10)
    assert len(alerts) == 0


def test_high_wind_alert_fires_when_wind_speed_exceeds_the_limit():
    station, _ = (
        StationBuilder()
        .with_thresholds(AlertThresholds(high_wind_speed_limit=50))
        .build()
    )
    alerts = station.record(20, 50, 60)
    assert len(alerts) == 1
    assert alerts[0].message == "High wind alert: 60 exceeds limit of 50."


def test_no_high_wind_alert_when_wind_speed_is_at_or_below_the_limit():
    station, _ = (
        StationBuilder()
        .with_thresholds(AlertThresholds(high_wind_speed_limit=50))
        .build()
    )
    alerts = station.record(20, 50, 50)
    assert len(alerts) == 0


def test_multiple_alerts_can_fire_for_a_single_reading():
    station, _ = (
        StationBuilder()
        .with_thresholds(
            AlertThresholds(high_temperature_ceiling=35, high_wind_speed_limit=50)
        )
        .build()
    )
    alerts = station.record(40, 50, 60)
    assert len(alerts) == 2
    messages = [a.message for a in alerts]
    assert "High temperature alert: 40 exceeds ceiling of 35." in messages
    assert "High wind alert: 60 exceeds limit of 50." in messages
