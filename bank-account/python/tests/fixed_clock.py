from __future__ import annotations

from datetime import date


class FixedClock:
    def __init__(self, current: date) -> None:
        self._current = current

    def today(self) -> date:
        return self._current

    def advance_to(self, d: date) -> None:
        self._current = d
