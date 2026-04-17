from __future__ import annotations

from dataclasses import dataclass
from datetime import datetime

from .spot_type import SpotType
from .vehicle import Vehicle


@dataclass(frozen=True)
class Ticket:
    vehicle: Vehicle
    spot_type: SpotType
    entry_time: datetime
