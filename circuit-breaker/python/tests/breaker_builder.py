from __future__ import annotations

from datetime import datetime, timedelta, timezone
from typing import Optional

from circuit_breaker import Breaker, DEFAULT_FAILURE_THRESHOLD, DEFAULT_RESET_TIMEOUT

from .fixed_clock import FixedClock


class BreakerBuilder:
    def __init__(self) -> None:
        self._threshold: int = DEFAULT_FAILURE_THRESHOLD
        self._timeout: timedelta = DEFAULT_RESET_TIMEOUT
        self._start: datetime = datetime(2026, 1, 1, tzinfo=timezone.utc)
        self._clock: Optional[FixedClock] = None

    def with_threshold(self, threshold: int) -> "BreakerBuilder":
        self._threshold = threshold
        return self

    def with_timeout(self, timeout: timedelta) -> "BreakerBuilder":
        self._timeout = timeout
        return self

    def starting_at(self, start: datetime) -> "BreakerBuilder":
        self._start = start
        return self

    def with_clock(self, clock: FixedClock) -> "BreakerBuilder":
        self._clock = clock
        return self

    def build(self) -> tuple[Breaker, FixedClock]:
        clock = self._clock if self._clock is not None else FixedClock(self._start)
        breaker = Breaker(self._threshold, self._timeout, clock)
        return breaker, clock
