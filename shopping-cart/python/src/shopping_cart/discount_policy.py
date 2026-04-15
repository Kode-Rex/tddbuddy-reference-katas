from __future__ import annotations

from dataclasses import dataclass
from decimal import Decimal
from typing import Protocol

from .money import Money
from .quantity import Quantity

MIN_PERCENT = 0
MAX_PERCENT = 100
HUNDRED_PERCENT = Decimal(100)


class DiscountPolicy(Protocol):
    def apply(self, unit_price: Money, quantity: Quantity) -> Money: ...


class NoDiscount:
    def apply(self, unit_price: Money, quantity: Quantity) -> Money:
        return unit_price * quantity.value


NO_DISCOUNT = NoDiscount()


@dataclass(frozen=True)
class PercentOff:
    percent: int

    def __post_init__(self) -> None:
        if not isinstance(self.percent, int) or isinstance(self.percent, bool):
            raise ValueError("Percent must be an integer between 0 and 100")
        if self.percent < MIN_PERCENT or self.percent > MAX_PERCENT:
            raise ValueError("Percent must be an integer between 0 and 100")

    def apply(self, unit_price: Money, quantity: Quantity) -> Money:
        gross = unit_price.amount * quantity.value
        multiplier = (HUNDRED_PERCENT - Decimal(self.percent)) / HUNDRED_PERCENT
        return Money(gross * multiplier)


@dataclass(frozen=True)
class FixedOff:
    amount: Money

    def __post_init__(self) -> None:
        if self.amount < Money.zero():
            raise ValueError("Fixed discount amount must not be negative")

    def apply(self, unit_price: Money, quantity: Quantity) -> Money:
        gross = unit_price * quantity.value
        discounted = gross - self.amount
        return Money.zero() if discounted < Money.zero() else discounted


@dataclass(frozen=True)
class BuyXGetY:
    buy_count: int
    free_count: int

    def __post_init__(self) -> None:
        if not isinstance(self.buy_count, int) or self.buy_count <= 0:
            raise ValueError("Buy count must be a positive integer")
        if not isinstance(self.free_count, int) or self.free_count <= 0:
            raise ValueError("Free count must be a positive integer")

    def apply(self, unit_price: Money, quantity: Quantity) -> Money:
        group_size = self.buy_count + self.free_count
        full_groups, remainder = divmod(quantity.value, group_size)
        chargeable = full_groups * self.buy_count + min(remainder, self.buy_count)
        return unit_price * chargeable


@dataclass(frozen=True)
class BulkPricing:
    threshold: Quantity
    bulk_unit_price: Money

    def __post_init__(self) -> None:
        if self.bulk_unit_price < Money.zero():
            raise ValueError("Bulk unit price must not be negative")

    def apply(self, unit_price: Money, quantity: Quantity) -> Money:
        effective = self.bulk_unit_price if quantity.value >= self.threshold.value else unit_price
        return effective * quantity.value
