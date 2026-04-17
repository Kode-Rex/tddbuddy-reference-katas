from __future__ import annotations

from heavy_metal_bake_sale import Money, Product


class ProductBuilder:
    def __init__(self) -> None:
        self._name = "Brownie"
        self._price = 0.75
        self._purchase_code = "B"
        self._stock = 48

    def with_name(self, name: str) -> ProductBuilder:
        self._name = name
        return self

    def with_price(self, price: float) -> ProductBuilder:
        self._price = price
        return self

    def with_purchase_code(self, code: str) -> ProductBuilder:
        self._purchase_code = code
        return self

    def with_stock(self, stock: int) -> ProductBuilder:
        self._stock = stock
        return self

    def build(self) -> Product:
        return Product(self._name, Money(self._price), self._purchase_code, self._stock)
