from __future__ import annotations

from decimal import Decimal

from .alert import Alert
from .alert_thresholds import AlertThresholds
from .clock import Clock
from .invalid_reading_error import InvalidReadingError
from .no_readings_error import NoReadingsError
from .reading import Reading
from .statistics import Statistics


class Station:
    def __init__(
        self,
        clock: Clock,
        thresholds: AlertThresholds | None = None,
    ) -> None:
        self._clock = clock
        self._thresholds = thresholds or AlertThresholds()
        self._readings: list[Reading] = []

    @property
    def readings(self) -> list[Reading]:
        return list(self._readings)

    def record(
        self,
        temperature: Decimal | int | float | str,
        humidity: Decimal | int | float | str,
        wind_speed: Decimal | int | float | str,
    ) -> list[Alert]:
        temp = Decimal(str(temperature))
        hum = Decimal(str(humidity))
        wind = Decimal(str(wind_speed))

        if hum < 0 or hum > 100:
            raise InvalidReadingError("Humidity must be between 0 and 100.")
        if wind < 0:
            raise InvalidReadingError("Wind speed must be non-negative.")

        reading = Reading(temp, hum, wind, self._clock.now())
        self._readings.append(reading)

        return self._evaluate_alerts(reading)

    def get_statistics(self) -> Statistics:
        if not self._readings:
            raise NoReadingsError("Cannot compute statistics with no readings.")

        temps = [r.temperature for r in self._readings]
        humidities = [r.humidity for r in self._readings]
        winds = [r.wind_speed for r in self._readings]
        count = Decimal(len(self._readings))

        return Statistics(
            min_temperature=min(temps),
            max_temperature=max(temps),
            avg_temperature=sum(temps) / count,
            min_humidity=min(humidities),
            max_humidity=max(humidities),
            avg_humidity=sum(humidities) / count,
            max_wind_speed=max(winds),
            avg_wind_speed=sum(winds) / count,
        )

    def _evaluate_alerts(self, reading: Reading) -> list[Alert]:
        alerts: list[Alert] = []

        if (
            self._thresholds.high_temperature_ceiling is not None
            and reading.temperature > self._thresholds.high_temperature_ceiling
        ):
            alerts.append(
                Alert(
                    f"High temperature alert: {reading.temperature} exceeds ceiling of {self._thresholds.high_temperature_ceiling}."
                )
            )

        if (
            self._thresholds.low_temperature_floor is not None
            and reading.temperature < self._thresholds.low_temperature_floor
        ):
            alerts.append(
                Alert(
                    f"Low temperature alert: {reading.temperature} is below floor of {self._thresholds.low_temperature_floor}."
                )
            )

        if (
            self._thresholds.high_wind_speed_limit is not None
            and reading.wind_speed > self._thresholds.high_wind_speed_limit
        ):
            alerts.append(
                Alert(
                    f"High wind alert: {reading.wind_speed} exceeds limit of {self._thresholds.high_wind_speed_limit}."
                )
            )

        return alerts
