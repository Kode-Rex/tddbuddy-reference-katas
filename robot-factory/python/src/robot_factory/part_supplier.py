from __future__ import annotations

from typing import Protocol

from .part_option import PartOption
from .part_quote import PartQuote
from .part_type import PartType
from .purchased_part import PurchasedPart


class PartSupplier(Protocol):
    @property
    def name(self) -> str: ...

    def get_quote(self, part_type: PartType, option: PartOption) -> PartQuote | None: ...

    def purchase(self, part_type: PartType, option: PartOption) -> PurchasedPart: ...
