from __future__ import annotations

from dataclasses import dataclass
from decimal import ROUND_HALF_UP, Decimal
from typing import Protocol

from .money import Money
from .weight import Weight


class PricingRule(Protocol):
    def calculate(self, quantity: int, weight: Weight) -> Money: ...


@dataclass(frozen=True)
class UnitPrice:
    """Each item costs a fixed amount."""

    price: Money

    def calculate(self, quantity: int, weight: Weight) -> Money:
        return self.price * quantity


@dataclass(frozen=True)
class MultiBuy:
    """Buy N items for a group price; remaining at individual price."""

    group_size: int
    group_price: Money
    item_price: Money

    def __post_init__(self) -> None:
        if self.group_size <= 0:
            raise ValueError("Group size must be positive")

    def calculate(self, quantity: int, weight: Weight) -> Money:
        full_groups, remainder = divmod(quantity, self.group_size)
        return self.group_price * full_groups + self.item_price * remainder


@dataclass(frozen=True)
class BuyOneGetOneFree:
    """Every second item is free."""

    item_price: Money

    def calculate(self, quantity: int, weight: Weight) -> Money:
        chargeable = (quantity + 1) // 2
        return self.item_price * chargeable


@dataclass(frozen=True)
class WeightedPrice:
    """Price per kilogram; charge rounded to nearest cent."""

    cents_per_kg: int

    def __post_init__(self) -> None:
        if self.cents_per_kg < 0:
            raise ValueError("Price per kg must not be negative")

    def calculate(self, quantity: int, weight: Weight) -> Money:
        raw = weight.kg * Decimal(self.cents_per_kg)
        rounded = int(raw.quantize(Decimal(1), rounding=ROUND_HALF_UP))
        return Money(rounded)
