from __future__ import annotations

from dataclasses import dataclass
from decimal import Decimal


@dataclass(frozen=True)
class Weight:
    """Non-negative weight in kilograms."""

    kg: Decimal = Decimal(0)

    def __init__(self, kg: Decimal | float | int | str = 0) -> None:
        value = Decimal(str(kg))
        if value < 0:
            raise ValueError("Weight must not be negative")
        object.__setattr__(self, "kg", value)

    @classmethod
    def zero(cls) -> Weight:
        return cls(0)
