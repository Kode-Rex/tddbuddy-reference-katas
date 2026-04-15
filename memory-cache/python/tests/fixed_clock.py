from __future__ import annotations

from datetime import datetime, timedelta


class FixedClock:
    def __init__(self, now: datetime) -> None:
        self._now = now

    def now(self) -> datetime:
        return self._now

    def advance_to(self, when: datetime) -> None:
        self._now = when

    def advance(self, delta: timedelta) -> None:
        self._now = self._now + delta
