from __future__ import annotations

from heavy_metal_bake_sale import BakeSale, Money, Product


class OrderBuilder:
    def __init__(self) -> None:
        self._inventory: list[Product] = []

    def with_product(self, product: Product) -> OrderBuilder:
        self._inventory.append(product)
        return self

    def with_default_inventory(self) -> OrderBuilder:
        self._inventory.append(Product("Brownie", Money(0.75), "B", 48))
        self._inventory.append(Product("Muffin", Money(1.00), "M", 36))
        self._inventory.append(Product("Cake Pop", Money(1.35), "C", 24))
        self._inventory.append(Product("Water", Money(1.50), "W", 30))
        return self

    def build(self) -> BakeSale:
        return BakeSale(self._inventory)
