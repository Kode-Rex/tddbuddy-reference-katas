from __future__ import annotations

from robot_factory import Money, PartOption, PartQuote, PartType, PurchasedPart


class FakePartSupplier:
    def __init__(self, name: str) -> None:
        self._name = name
        self._catalog: dict[tuple[PartType, PartOption], Money] = {}
        self._purchase_log: list[PurchasedPart] = []

    @property
    def name(self) -> str:
        return self._name

    @property
    def purchase_log(self) -> list[PurchasedPart]:
        return list(self._purchase_log)

    def with_part(
        self, part_type: PartType, option: PartOption, price: Money
    ) -> FakePartSupplier:
        self._catalog[(part_type, option)] = price
        return self

    def get_quote(
        self, part_type: PartType, option: PartOption
    ) -> PartQuote | None:
        price = self._catalog.get((part_type, option))
        if price is None:
            return None
        return PartQuote(
            type=part_type, option=option, price=price, supplier_name=self._name
        )

    def purchase(
        self, part_type: PartType, option: PartOption
    ) -> PurchasedPart:
        price = self._catalog[(part_type, option)]
        part = PurchasedPart(
            type=part_type, option=option, price=price, supplier_name=self._name
        )
        self._purchase_log.append(part)
        return part
