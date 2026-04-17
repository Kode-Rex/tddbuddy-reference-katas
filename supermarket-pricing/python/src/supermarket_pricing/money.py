from __future__ import annotations

from dataclasses import dataclass


@dataclass(frozen=True)
class Money:
    """Monetary amount in cents. Integer arithmetic avoids floating-point rounding."""

    cents: int = 0

    @classmethod
    def zero(cls) -> Money:
        return cls(0)

    def __add__(self, other: Money) -> Money:
        return Money(self.cents + other.cents)

    def __sub__(self, other: Money) -> Money:
        return Money(self.cents - other.cents)

    def __mul__(self, factor: int) -> Money:
        return Money(self.cents * factor)
