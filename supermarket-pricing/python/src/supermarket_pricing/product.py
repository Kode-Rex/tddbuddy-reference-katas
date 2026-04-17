from __future__ import annotations

from dataclasses import dataclass

from .pricing_rule import PricingRule


@dataclass(frozen=True)
class Product:
    sku: str
    name: str
    pricing_rule: PricingRule

    def __post_init__(self) -> None:
        if not self.sku or not self.sku.strip():
            raise ValueError("SKU must not be empty")
