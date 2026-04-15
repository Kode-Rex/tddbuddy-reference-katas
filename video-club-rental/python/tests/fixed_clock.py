from __future__ import annotations

from datetime import date, timedelta


class FixedClock:
    def __init__(self, today: date) -> None:
        self._today = today

    def today(self) -> date:
        return self._today

    def advance_to(self, d: date) -> None:
        self._today = d

    def advance_days(self, days: int) -> None:
        self._today = self._today + timedelta(days=days)
