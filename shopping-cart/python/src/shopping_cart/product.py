from __future__ import annotations

from dataclasses import dataclass, field

from .discount_policy import NO_DISCOUNT, DiscountPolicy
from .money import Money


@dataclass(frozen=True)
class Product:
    sku: str
    name: str
    unit_price: Money
    discount_policy: DiscountPolicy = field(default_factory=lambda: NO_DISCOUNT)

    def __post_init__(self) -> None:
        if not self.sku or not self.sku.strip():
            raise ValueError("SKU must not be empty")
