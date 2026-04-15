from __future__ import annotations

from dataclasses import dataclass
from datetime import datetime, timedelta

from .clock import Clock
from .decision import Allowed, Decision, Rejected
from .rule import Rule

DEFAULT_MAX_REQUESTS = 100
DEFAULT_WINDOW_DURATION = timedelta(seconds=60)


@dataclass
class _WindowState:
    start: datetime
    count: int


class Limiter:
    def __init__(self, rule: Rule, clock: Clock) -> None:
        self._rule = rule
        self._clock = clock
        self._windows: dict[str, _WindowState] = {}

    def request(self, key: str) -> Decision:
        now = self._clock.now()
        state = self._windows.get(key)

        if state is None or now >= state.start + self._rule.window:
            self._windows[key] = _WindowState(now, 1)
            return Allowed()

        if state.count < self._rule.max_requests:
            state.count += 1
            return Allowed()

        retry_after = (state.start + self._rule.window) - now
        return Rejected(retry_after)
