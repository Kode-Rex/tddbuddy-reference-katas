from __future__ import annotations

from robot_factory import Money, PartOption, PartType

from .fake_part_supplier import FakePartSupplier


class SupplierBuilder:
    def __init__(self) -> None:
        self._name = "Supplier"
        self._parts: list[tuple[PartType, PartOption, float]] = []

    def named(self, name: str) -> SupplierBuilder:
        self._name = name
        return self

    def with_part(
        self, part_type: PartType, option: PartOption, price: float
    ) -> SupplierBuilder:
        self._parts.append((part_type, option, price))
        return self

    def build(self) -> FakePartSupplier:
        supplier = FakePartSupplier(self._name)
        for part_type, option, price in self._parts:
            supplier.with_part(part_type, option, Money(price))
        return supplier
