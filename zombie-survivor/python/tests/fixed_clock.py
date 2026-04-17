from datetime import datetime


class FixedClock:
    def __init__(self, now: datetime) -> None:
        self._now = now

    def now(self) -> datetime:
        return self._now

    def advance_to(self, dt: datetime) -> None:
        self._now = dt
