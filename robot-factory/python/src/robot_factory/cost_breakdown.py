from __future__ import annotations

from dataclasses import dataclass

from .money import Money
from .part_quote import PartQuote


@dataclass(frozen=True)
class CostBreakdown:
    parts: list[PartQuote]
    total: Money
