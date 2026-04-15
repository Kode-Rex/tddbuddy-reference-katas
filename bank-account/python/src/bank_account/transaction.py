from __future__ import annotations

from dataclasses import dataclass
from datetime import date

from .money import Money


@dataclass(frozen=True)
class Transaction:
    date: date
    amount: Money
    balance_after: Money
