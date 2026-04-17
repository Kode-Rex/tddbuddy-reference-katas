from __future__ import annotations

from datetime import datetime, timedelta


class FixedClock:
    def __init__(self, current: datetime) -> None:
        self._current = current

    def now(self) -> datetime:
        return self._current

    def advance_to(self, dt: datetime) -> None:
        self._current = dt

    def advance_by_minutes(self, minutes: int) -> None:
        self._current = self._current + timedelta(minutes=minutes)
