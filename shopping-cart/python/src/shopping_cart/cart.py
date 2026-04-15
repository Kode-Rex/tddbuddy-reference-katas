from __future__ import annotations

from typing import Optional

from .line_item import LineItem
from .money import Money
from .product import Product
from .quantity import Quantity

_ONE = Quantity(1)


class Cart:
    def __init__(self) -> None:
        self._lines: list[LineItem] = []

    @property
    def lines(self) -> list[LineItem]:
        return list(self._lines)

    @property
    def is_empty(self) -> bool:
        return len(self._lines) == 0

    def add(self, product: Product, quantity: Quantity = _ONE) -> None:
        existing = self._find_line(product.sku)
        if existing is None:
            self._lines.append(LineItem(product, quantity))
        else:
            existing.increment_by(quantity)

    def remove(self, sku: str) -> None:
        existing = self._find_line(sku)
        if existing is not None:
            self._lines.remove(existing)

    def update_quantity(self, sku: str, quantity: int) -> None:
        line = self._find_line(sku)
        if line is None:
            raise LookupError(f"No line item for SKU '{sku}'")
        line.set_quantity(Quantity(quantity))

    def total(self) -> Money:
        total = Money.zero()
        for line in self._lines:
            total = total + line.subtotal()
        return total

    def _find_line(self, sku: str) -> Optional[LineItem]:
        return next((l for l in self._lines if l.product.sku == sku), None)
