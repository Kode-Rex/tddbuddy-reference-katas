from __future__ import annotations

from .money import Money
from .product import Product
from .quantity import Quantity


class LineItem:
    def __init__(self, product: Product, quantity: Quantity) -> None:
        self._product = product
        self._quantity = quantity

    @property
    def product(self) -> Product:
        return self._product

    @property
    def quantity(self) -> Quantity:
        return self._quantity

    def subtotal(self) -> Money:
        return self._product.discount_policy.apply(self._product.unit_price, self._quantity)

    def increment_by(self, delta: Quantity) -> None:
        self._quantity = self._quantity + delta

    def set_quantity(self, quantity: Quantity) -> None:
        self._quantity = quantity
