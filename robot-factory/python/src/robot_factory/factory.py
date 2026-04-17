from __future__ import annotations

from functools import reduce

from .cost_breakdown import CostBreakdown
from .money import Money
from .part_not_available_error import PartNotAvailableError
from .part_supplier import PartSupplier
from .purchased_part import PurchasedPart
from .robot_order import RobotOrder


class Factory:
    def __init__(self, suppliers: list[PartSupplier]) -> None:
        self._suppliers = list(suppliers)

    def cost_robot(self, order: RobotOrder) -> CostBreakdown:
        order.validate()

        cheapest_parts = []

        for part_type, option in order.parts.items():
            quotes = [
                q
                for s in self._suppliers
                if (q := s.get_quote(part_type, option)) is not None
            ]

            if not quotes:
                raise PartNotAvailableError(
                    f"Part not available: {option.value}"
                )

            cheapest_parts.append(min(quotes, key=lambda q: q.price))

        total = reduce(lambda s, q: s + q.price, cheapest_parts, Money.zero())
        return CostBreakdown(parts=cheapest_parts, total=total)

    def purchase_robot(self, order: RobotOrder) -> list[PurchasedPart]:
        breakdown = self.cost_robot(order)

        purchased = []
        for quote in breakdown.parts:
            supplier = next(
                s for s in self._suppliers if s.name == quote.supplier_name
            )
            purchased.append(supplier.purchase(quote.type, quote.option))

        return purchased
