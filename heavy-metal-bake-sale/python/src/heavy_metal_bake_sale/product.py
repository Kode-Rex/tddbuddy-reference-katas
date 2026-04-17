from __future__ import annotations

from .money import Money


class Product:
    def __init__(self, name: str, price: Money, purchase_code: str, stock: int) -> None:
        self._name = name
        self._price = price
        self._purchase_code = purchase_code
        self._stock = stock

    @property
    def name(self) -> str:
        return self._name

    @property
    def price(self) -> Money:
        return self._price

    @property
    def purchase_code(self) -> str:
        return self._purchase_code

    @property
    def stock(self) -> int:
        return self._stock

    @property
    def is_in_stock(self) -> bool:
        return self._stock > 0

    def decrement_stock(self) -> None:
        self._stock -= 1
