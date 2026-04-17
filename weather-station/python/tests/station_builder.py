from __future__ import annotations

from dataclasses import dataclass
from datetime import datetime, timezone

from weather_station import AlertThresholds, Station

from .fixed_clock import FixedClock


@dataclass
class _SeededReading:
    time: datetime
    temperature: float
    humidity: float
    wind_speed: float


class StationBuilder:
    def __init__(self) -> None:
        self._start_time = datetime(2026, 6, 15, 12, 0, 0, tzinfo=timezone.utc)
        self._thresholds: AlertThresholds | None = None
        self._seeded: list[_SeededReading] = []

    def starting_at(self, time: datetime) -> StationBuilder:
        self._start_time = time
        return self

    def with_thresholds(self, thresholds: AlertThresholds) -> StationBuilder:
        self._thresholds = thresholds
        return self

    def with_reading(
        self,
        temperature: float,
        humidity: float,
        wind_speed: float,
        at: datetime | None = None,
    ) -> StationBuilder:
        self._seeded.append(
            _SeededReading(at or self._start_time, temperature, humidity, wind_speed)
        )
        return self

    def build(self) -> tuple[Station, FixedClock]:
        clock = FixedClock(self._start_time)
        station = Station(clock, self._thresholds)
        for seed in self._seeded:
            clock.advance_to(seed.time)
            station.record(seed.temperature, seed.humidity, seed.wind_speed)
        return station, clock
