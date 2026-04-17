from __future__ import annotations

from dataclasses import dataclass
from decimal import Decimal


@dataclass(frozen=True)
class AlertThresholds:
    high_temperature_ceiling: Decimal | None = None
    low_temperature_floor: Decimal | None = None
    high_wind_speed_limit: Decimal | None = None

    def __init__(
        self,
        high_temperature_ceiling: Decimal | int | float | str | None = None,
        low_temperature_floor: Decimal | int | float | str | None = None,
        high_wind_speed_limit: Decimal | int | float | str | None = None,
    ) -> None:
        object.__setattr__(
            self,
            "high_temperature_ceiling",
            Decimal(str(high_temperature_ceiling)) if high_temperature_ceiling is not None else None,
        )
        object.__setattr__(
            self,
            "low_temperature_floor",
            Decimal(str(low_temperature_floor)) if low_temperature_floor is not None else None,
        )
        object.__setattr__(
            self,
            "high_wind_speed_limit",
            Decimal(str(high_wind_speed_limit)) if high_wind_speed_limit is not None else None,
        )
