from __future__ import annotations

from dataclasses import dataclass

from supermarket_pricing import Checkout, ComboDeal, Money, Product, Weight


@dataclass
class _Scanned:
    product: Product
    count: int


@dataclass
class _Weighed:
    product: Product
    weight: Weight


class CheckoutBuilder:
    def __init__(self) -> None:
        self._scanned: list[_Scanned] = []
        self._weighed: list[_Weighed] = []
        self._combo_deals: list[ComboDeal] = []

    def with_scanned(self, product: Product, count: int = 1) -> CheckoutBuilder:
        self._scanned.append(_Scanned(product, count))
        return self

    def with_weighed(self, product: Product, kg: float | str) -> CheckoutBuilder:
        self._weighed.append(_Weighed(product, Weight(kg)))
        return self

    def with_combo_deal(self, sku_a: str, sku_b: str, deal_cents: int) -> CheckoutBuilder:
        self._combo_deals.append(ComboDeal(sku_a, sku_b, Money(deal_cents)))
        return self

    def build(self) -> Checkout:
        checkout = Checkout(self._combo_deals)
        for s in self._scanned:
            for _ in range(s.count):
                checkout.scan(s.product)
        for w in self._weighed:
            checkout.scan_weighted(w.product, w.weight)
        return checkout
