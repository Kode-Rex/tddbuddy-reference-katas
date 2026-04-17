from __future__ import annotations

from dataclasses import dataclass
from datetime import datetime

from .money import Money


@dataclass(frozen=True)
class AccountEvent:
    account_id: str
    timestamp: datetime


@dataclass(frozen=True)
class AccountOpened(AccountEvent):
    owner_name: str = ""


@dataclass(frozen=True)
class MoneyDeposited(AccountEvent):
    amount: Money = Money.zero()


@dataclass(frozen=True)
class MoneyWithdrawn(AccountEvent):
    amount: Money = Money.zero()


@dataclass(frozen=True)
class AccountClosed(AccountEvent):
    pass
