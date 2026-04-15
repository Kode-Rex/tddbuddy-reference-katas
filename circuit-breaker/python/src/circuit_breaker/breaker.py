from __future__ import annotations

from datetime import datetime, timedelta
from typing import Callable, TypeVar

from .breaker_state import BreakerState
from .clock import Clock
from .exceptions import (
    BreakerThresholdInvalidError,
    BreakerTimeoutInvalidError,
    CircuitOpenError,
)

T = TypeVar("T")

DEFAULT_FAILURE_THRESHOLD = 5
DEFAULT_RESET_TIMEOUT = timedelta(seconds=30)


class Breaker:
    def __init__(
        self,
        failure_threshold: int,
        reset_timeout: timedelta,
        clock: Clock,
    ) -> None:
        if failure_threshold <= 0:
            raise BreakerThresholdInvalidError("Failure threshold must be positive")
        if reset_timeout <= timedelta(0):
            raise BreakerTimeoutInvalidError("Reset timeout must be positive")
        self._failure_threshold = failure_threshold
        self._reset_timeout = reset_timeout
        self._clock = clock
        self._state: BreakerState = BreakerState.CLOSED
        self._consecutive_failures = 0
        self._opened_at: datetime | None = None

    @property
    def state(self) -> BreakerState:
        return self._state

    def execute(self, operation: Callable[[], T]) -> T:
        if self._state is BreakerState.OPEN:
            assert self._opened_at is not None
            if self._clock.now() - self._opened_at >= self._reset_timeout:
                self._state = BreakerState.HALF_OPEN
            else:
                raise CircuitOpenError("Circuit is open")

        try:
            result = operation()
        except BaseException:
            self._on_failure()
            raise
        self._on_success()
        return result

    def _on_success(self) -> None:
        self._consecutive_failures = 0
        self._state = BreakerState.CLOSED

    def _on_failure(self) -> None:
        if self._state is BreakerState.HALF_OPEN:
            self._trip()
            return
        self._consecutive_failures += 1
        if self._consecutive_failures >= self._failure_threshold:
            self._trip()

    def _trip(self) -> None:
        self._state = BreakerState.OPEN
        self._opened_at = self._clock.now()
