from __future__ import annotations

from datetime import datetime, timedelta, timezone
from typing import Optional

from rate_limiter import (
    DEFAULT_MAX_REQUESTS,
    DEFAULT_WINDOW_DURATION,
    Limiter,
    Rule,
)

from .fixed_clock import FixedClock


class LimiterBuilder:
    def __init__(self) -> None:
        self._max_requests: int = DEFAULT_MAX_REQUESTS
        self._window: timedelta = DEFAULT_WINDOW_DURATION
        self._start: datetime = datetime(2026, 1, 1, tzinfo=timezone.utc)
        self._clock: Optional[FixedClock] = None

    def with_max_requests(self, max_requests: int) -> "LimiterBuilder":
        self._max_requests = max_requests
        return self

    def with_window(self, window: timedelta) -> "LimiterBuilder":
        self._window = window
        return self

    def starting_at(self, start: datetime) -> "LimiterBuilder":
        self._start = start
        return self

    def with_clock(self, clock: FixedClock) -> "LimiterBuilder":
        self._clock = clock
        return self

    def build(self) -> tuple[Limiter, FixedClock]:
        clock = self._clock if self._clock is not None else FixedClock(self._start)
        limiter = Limiter(Rule(self._max_requests, self._window), clock)
        return limiter, clock
