from __future__ import annotations

from datetime import datetime
from typing import Protocol


class MachineDevice(Protocol):
    def lock(self, reservation_id: str, reservation_date_time: datetime, pin: int) -> bool: ...
    def unlock(self, reservation_id: str) -> None: ...
