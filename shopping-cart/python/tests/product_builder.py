from __future__ import annotations

from typing import Optional

from shopping_cart import DiscountPolicy, Money, Product


class ProductBuilder:
    def __init__(self) -> None:
        self._sku = "SKU-DEFAULT"
        self._name = "Widget"
        self._unit_price = Money(10)
        self._discount_policy: Optional[DiscountPolicy] = None

    def with_sku(self, sku: str) -> "ProductBuilder":
        self._sku = sku
        return self

    def named(self, name: str) -> "ProductBuilder":
        self._name = name
        return self

    def priced_at(self, amount: float | str | int) -> "ProductBuilder":
        self._unit_price = Money(amount)
        return self

    def with_discount(self, policy: DiscountPolicy) -> "ProductBuilder":
        self._discount_policy = policy
        return self

    def build(self) -> Product:
        if self._discount_policy is None:
            return Product(self._sku, self._name, self._unit_price)
        return Product(self._sku, self._name, self._unit_price, self._discount_policy)
