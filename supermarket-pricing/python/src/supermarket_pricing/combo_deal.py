from __future__ import annotations

from dataclasses import dataclass

from .money import Money


@dataclass(frozen=True)
class ComboDeal:
    """Cross-product deal: one of each SKU at a special price."""

    sku_a: str
    sku_b: str
    deal_price: Money
