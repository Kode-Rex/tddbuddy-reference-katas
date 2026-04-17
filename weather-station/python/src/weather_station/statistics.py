from __future__ import annotations

from dataclasses import dataclass
from decimal import Decimal


@dataclass(frozen=True)
class Statistics:
    min_temperature: Decimal
    max_temperature: Decimal
    avg_temperature: Decimal
    min_humidity: Decimal
    max_humidity: Decimal
    avg_humidity: Decimal
    max_wind_speed: Decimal
    avg_wind_speed: Decimal
