from __future__ import annotations

from dataclasses import dataclass

from .account_status import AccountStatus
from .money import Money


@dataclass(frozen=True)
class AccountSummary:
    owner_name: str
    balance: Money
    transaction_count: int
    status: AccountStatus
