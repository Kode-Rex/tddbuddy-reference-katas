from __future__ import annotations

from .clock import Clock
from .money import Money
from .transaction import Transaction


class Account:
    def __init__(self, clock: Clock) -> None:
        self._clock = clock
        self._balance = Money.zero()
        self._transactions: list[Transaction] = []

    @property
    def balance(self) -> Money:
        return self._balance

    @property
    def transactions(self) -> list[Transaction]:
        return list(self._transactions)

    def deposit(self, amount: Money) -> bool:
        if not amount.is_positive:
            return False
        self._balance = self._balance + amount
        self._transactions.append(
            Transaction(self._clock.today(), amount, self._balance)
        )
        return True

    def withdraw(self, amount: Money) -> bool:
        if not amount.is_positive:
            return False
        if amount > self._balance:
            return False
        self._balance = self._balance - amount
        self._transactions.append(
            Transaction(self._clock.today(), -amount, self._balance)
        )
        return True

    def print_statement(self) -> str:
        header = "Date       | Amount  | Balance"
        lines = [header]
        for t in self._transactions:
            date_str = t.date.isoformat()
            amount_str = f"{t.amount.amount:.2f}".rjust(7)
            balance_str = f"{t.balance_after.amount:.2f}".rjust(7)
            lines.append(f"{date_str} | {amount_str} | {balance_str}")
        return "\n".join(lines)
