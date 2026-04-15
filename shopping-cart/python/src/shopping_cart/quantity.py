from __future__ import annotations

from dataclasses import dataclass


@dataclass(frozen=True)
class Quantity:
    value: int

    def __post_init__(self) -> None:
        if not isinstance(self.value, int) or isinstance(self.value, bool) or self.value <= 0:
            raise ValueError("Quantity must be a positive whole number")

    def __add__(self, other: "Quantity") -> "Quantity":
        return Quantity(self.value + other.value)
