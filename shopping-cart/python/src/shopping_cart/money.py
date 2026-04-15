from __future__ import annotations

from dataclasses import dataclass, field
from decimal import Decimal


@dataclass(frozen=True)
class Money:
    amount: Decimal = field(default=Decimal(0))

    def __init__(self, amount: Decimal | int | float | str = 0) -> None:
        object.__setattr__(self, "amount", Decimal(str(amount)))

    @classmethod
    def zero(cls) -> "Money":
        return cls(0)

    def __add__(self, other: "Money") -> "Money":
        return Money(self.amount + other.amount)

    def __sub__(self, other: "Money") -> "Money":
        return Money(self.amount - other.amount)

    def __mul__(self, factor: int) -> "Money":
        return Money(self.amount * factor)

    def __lt__(self, other: "Money") -> bool:
        return self.amount < other.amount

    def __gt__(self, other: "Money") -> bool:
        return self.amount > other.amount
