from __future__ import annotations

from dataclasses import dataclass

from .money import Money
from .part_option import PartOption
from .part_type import PartType


@dataclass(frozen=True)
class PartQuote:
    type: PartType
    option: PartOption
    price: Money
    supplier_name: str
