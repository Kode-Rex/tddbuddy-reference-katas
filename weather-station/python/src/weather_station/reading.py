from __future__ import annotations

from dataclasses import dataclass
from datetime import datetime
from decimal import Decimal


@dataclass(frozen=True)
class Reading:
    temperature: Decimal
    humidity: Decimal
    wind_speed: Decimal
    timestamp: datetime

    def __init__(
        self,
        temperature: Decimal | int | float | str,
        humidity: Decimal | int | float | str,
        wind_speed: Decimal | int | float | str,
        timestamp: datetime,
    ) -> None:
        object.__setattr__(self, "temperature", Decimal(str(temperature)))
        object.__setattr__(self, "humidity", Decimal(str(humidity)))
        object.__setattr__(self, "wind_speed", Decimal(str(wind_speed)))
        object.__setattr__(self, "timestamp", timestamp)
