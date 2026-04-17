from __future__ import annotations

from supermarket_pricing import (
    BuyOneGetOneFree,
    Money,
    MultiBuy,
    PricingRule,
    Product,
    UnitPrice,
    WeightedPrice,
)


class ProductBuilder:
    def __init__(self) -> None:
        self._sku = "X"
        self._name = "Default Item"
        self._pricing_rule: PricingRule = UnitPrice(Money(100))

    def with_sku(self, sku: str) -> ProductBuilder:
        self._sku = sku
        return self

    def named(self, name: str) -> ProductBuilder:
        self._name = name
        return self

    def with_unit_price(self, cents: int) -> ProductBuilder:
        self._pricing_rule = UnitPrice(Money(cents))
        return self

    def with_multi_buy(self, group_size: int, group_cents: int, item_cents: int) -> ProductBuilder:
        self._pricing_rule = MultiBuy(group_size, Money(group_cents), Money(item_cents))
        return self

    def with_buy_one_get_one_free(self, item_cents: int) -> ProductBuilder:
        self._pricing_rule = BuyOneGetOneFree(Money(item_cents))
        return self

    def with_weighted_price(self, cents_per_kg: int) -> ProductBuilder:
        self._pricing_rule = WeightedPrice(cents_per_kg)
        return self

    def with_pricing_rule(self, rule: PricingRule) -> ProductBuilder:
        self._pricing_rule = rule
        return self

    def build(self) -> Product:
        return Product(self._sku, self._name, self._pricing_rule)
