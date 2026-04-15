from __future__ import annotations

from enum import Enum


class BreakerState(Enum):
    CLOSED = "Closed"
    OPEN = "Open"
    HALF_OPEN = "HalfOpen"
