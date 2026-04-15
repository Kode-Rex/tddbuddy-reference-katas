from __future__ import annotations

from dataclasses import dataclass

from shopping_cart import Cart, Product, Quantity


@dataclass
class _Seed:
    product: Product
    quantity: Quantity


class CartBuilder:
    def __init__(self) -> None:
        self._seeded: list[_Seed] = []

    def with_product(self, product: Product, quantity: Quantity | None = None) -> "CartBuilder":
        self._seeded.append(_Seed(product, quantity if quantity is not None else Quantity(1)))
        return self

    def build(self) -> Cart:
        cart = Cart()
        for seed in self._seeded:
            cart.add(seed.product, seed.quantity)
        return cart
