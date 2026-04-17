from __future__ import annotations

from dataclasses import dataclass
from datetime import datetime

from .money import Money


@dataclass(frozen=True)
class Transaction:
    timestamp: datetime
    amount: Money
    balance_after: Money
