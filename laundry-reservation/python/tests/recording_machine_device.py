from __future__ import annotations

from dataclasses import dataclass
from datetime import datetime


@dataclass
class LockCall:
    reservation_id: str
    reservation_date_time: datetime
    pin: int


class RecordingMachineDevice:
    def __init__(self) -> None:
        self.lock_calls: list[LockCall] = []
        self.unlock_calls: list[str] = []
        self.should_accept_lock: bool = True

    def lock(self, reservation_id: str, reservation_date_time: datetime, pin: int) -> bool:
        self.lock_calls.append(LockCall(reservation_id, reservation_date_time, pin))
        return self.should_accept_lock

    def unlock(self, reservation_id: str) -> None:
        self.unlock_calls.append(reservation_id)
