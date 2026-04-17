from __future__ import annotations

from collections import Counter

from .money import Money
from .product import Product
from .exceptions import (
    InsufficientPaymentException,
    OutOfStockException,
    UnknownPurchaseCodeException,
)


class BakeSale:
    def __init__(self, inventory: list[Product]) -> None:
        self._inventory = inventory

    @property
    def inventory(self) -> list[Product]:
        return list(self._inventory)

    @classmethod
    def create_default(cls) -> BakeSale:
        return cls([
            Product("Brownie", Money(0.75), "B", 48),
            Product("Muffin", Money(1.00), "M", 36),
            Product("Cake Pop", Money(1.35), "C", 24),
            Product("Water", Money(1.50), "W", 30),
        ])

    def calculate_total(self, order: str) -> Money:
        codes = [c.strip() for c in order.split(",")]
        products = self._resolve_products(codes)

        self._validate_stock(products)

        total = Money.zero()
        for product in products:
            total = total + product.price

        for product in products:
            product.decrement_stock()

        return total

    def calculate_change(self, total: Money, payment: Money) -> Money:
        if payment < total:
            raise InsufficientPaymentException()
        return payment - total

    def _resolve_products(self, codes: list[str]) -> list[Product]:
        products: list[Product] = []
        for code in codes:
            product = next(
                (p for p in self._inventory if p.purchase_code == code), None
            )
            if product is None:
                raise UnknownPurchaseCodeException(code)
            products.append(product)
        return products

    def _validate_stock(self, products: list[Product]) -> None:
        counts = Counter(p.purchase_code for p in products)
        for code, count in counts.items():
            product = next(p for p in products if p.purchase_code == code)
            if product.stock < count:
                raise OutOfStockException(product.name)
