from __future__ import annotations

from dataclasses import dataclass
from datetime import date

from bank_account import Account, Money

from .fixed_clock import FixedClock


@dataclass
class _SeededDeposit:
    when: date
    amount: float


class AccountBuilder:
    def __init__(self) -> None:
        self._opened_on: date = date(2026, 1, 1)
        self._seeded: list[_SeededDeposit] = []

    def opened_on(self, d: date) -> AccountBuilder:
        self._opened_on = d
        return self

    def with_deposit_on(self, d: date, amount: float) -> AccountBuilder:
        self._seeded.append(_SeededDeposit(d, amount))
        return self

    def build(self) -> tuple[Account, FixedClock]:
        clock = FixedClock(self._opened_on)
        account = Account(clock)
        for seed in self._seeded:
            clock.advance_to(seed.when)
            account.deposit(Money(seed.amount))
        return account, clock
