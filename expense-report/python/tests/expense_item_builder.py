from __future__ import annotations

from expense_report import Category, ExpenseItem, Money


class ExpenseItemBuilder:
    def __init__(self) -> None:
        self._description = "Office supplies"
        self._amount = 25.0
        self._category = Category.OTHER

    def with_description(self, description: str) -> ExpenseItemBuilder:
        self._description = description
        return self

    def with_amount(self, amount: float) -> ExpenseItemBuilder:
        self._amount = amount
        return self

    def with_category(self, category: Category) -> ExpenseItemBuilder:
        self._category = category
        return self

    def as_meal(self, amount: float = 30.0) -> ExpenseItemBuilder:
        return self.with_category(Category.MEALS).with_amount(amount).with_description("Team lunch")

    def as_travel(self, amount: float = 200.0) -> ExpenseItemBuilder:
        return self.with_category(Category.TRAVEL).with_amount(amount).with_description("Flight")

    def as_accommodation(self, amount: float = 150.0) -> ExpenseItemBuilder:
        return self.with_category(Category.ACCOMMODATION).with_amount(amount).with_description("Hotel stay")

    def as_equipment(self, amount: float = 800.0) -> ExpenseItemBuilder:
        return self.with_category(Category.EQUIPMENT).with_amount(amount).with_description("Laptop")

    def build(self) -> ExpenseItem:
        return ExpenseItem(self._description, Money(self._amount), self._category)
