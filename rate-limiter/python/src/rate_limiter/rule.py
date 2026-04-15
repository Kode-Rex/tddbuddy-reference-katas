from __future__ import annotations

from dataclasses import dataclass
from datetime import timedelta

from .exceptions import LimiterRuleInvalidError


@dataclass(frozen=True)
class Rule:
    max_requests: int
    window: timedelta

    def __post_init__(self) -> None:
        if self.max_requests <= 0:
            raise LimiterRuleInvalidError("Max requests must be positive")
        if self.window <= timedelta(0):
            raise LimiterRuleInvalidError("Window must be positive")
