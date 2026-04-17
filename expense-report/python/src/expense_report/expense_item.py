from __future__ import annotations

from dataclasses import dataclass

from .category import Category
from .money import Money
from . import spending_policy


@dataclass(frozen=True)
class ExpenseItem:
    description: str
    amount: Money
    category: Category

    @property
    def is_over_limit(self) -> bool:
        return self.amount > spending_policy.limit_for(self.category)
