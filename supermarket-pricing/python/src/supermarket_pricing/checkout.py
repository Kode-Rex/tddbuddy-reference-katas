from __future__ import annotations

from collections import defaultdict

from .combo_deal import ComboDeal
from .money import Money
from .product import Product
from .weight import Weight


class Checkout:
    def __init__(self, combo_deals: list[ComboDeal] | None = None) -> None:
        self._products: dict[str, Product] = {}
        self._quantities: dict[str, int] = defaultdict(int)
        self._weights: dict[str, Weight] = {}
        self._combo_deals: list[ComboDeal] = list(combo_deals) if combo_deals else []

    def scan(self, product: Product) -> None:
        self._ensure_registered(product)
        self._quantities[product.sku] += 1

    def scan_weighted(self, product: Product, weight: Weight) -> None:
        self._ensure_registered(product)
        existing = self._weights.get(product.sku, Weight.zero())
        self._weights[product.sku] = Weight(existing.kg + weight.kg)

    def total(self) -> Money:
        result = Money.zero()

        combo_consumed: dict[str, int] = defaultdict(int)

        for deal in self._combo_deals:
            count_a = self._quantities.get(deal.sku_a, 0) - combo_consumed.get(deal.sku_a, 0)
            count_b = self._quantities.get(deal.sku_b, 0) - combo_consumed.get(deal.sku_b, 0)
            pairs = min(count_a, count_b)

            if pairs > 0:
                result = result + deal.deal_price * pairs
                combo_consumed[deal.sku_a] += pairs
                combo_consumed[deal.sku_b] += pairs

        for sku, product in self._products.items():
            quantity = self._quantities.get(sku, 0) - combo_consumed.get(sku, 0)
            weight = self._weights.get(sku, Weight.zero())

            if quantity > 0 or weight.kg > 0:
                result = result + product.pricing_rule.calculate(quantity, weight)

        return result

    def _ensure_registered(self, product: Product) -> None:
        if product.sku not in self._products:
            self._products[product.sku] = product
