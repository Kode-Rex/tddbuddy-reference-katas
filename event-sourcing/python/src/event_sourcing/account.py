from __future__ import annotations

from datetime import datetime

from .account_status import AccountStatus
from .account_summary import AccountSummary
from .events import AccountEvent, AccountOpened, MoneyDeposited, MoneyWithdrawn, AccountClosed
from .exceptions import (
    AccountNotOpenException,
    AccountClosedException,
    InsufficientFundsException,
    InvalidAmountException,
    NonZeroBalanceException,
)
from .money import Money
from .transaction import Transaction


class Account:
    def __init__(self, account_id: str) -> None:
        self._account_id = account_id
        self._owner_name = ""
        self._balance = Money.zero()
        self._status = AccountStatus.OPEN
        self._opened = False
        self._events: list[AccountEvent] = []

    @property
    def account_id(self) -> str:
        return self._account_id

    @property
    def balance(self) -> Money:
        return self._balance

    @property
    def status(self) -> AccountStatus:
        return self._status

    @staticmethod
    def rebuild(events: list[AccountEvent]) -> Account:
        if not events:
            raise AccountNotOpenException("Account has no events.")

        first = events[0]
        if not isinstance(first, AccountOpened):
            raise AccountNotOpenException("Account was never opened.")

        account = Account(first.account_id)
        for e in events:
            account._apply(e)
        return account

    def deposit(self, amount: Money, timestamp: datetime) -> AccountEvent:
        self._ensure_open()
        if not amount.is_positive:
            raise InvalidAmountException("Deposit amount must be positive.")

        e = MoneyDeposited(
            account_id=self._account_id, timestamp=timestamp, amount=amount
        )
        self._apply(e)
        return e

    def withdraw(self, amount: Money, timestamp: datetime) -> AccountEvent:
        self._ensure_open()
        if not amount.is_positive:
            raise InvalidAmountException("Withdrawal amount must be positive.")
        if amount > self._balance:
            raise InsufficientFundsException(
                "Withdrawal amount exceeds current balance."
            )

        e = MoneyWithdrawn(
            account_id=self._account_id, timestamp=timestamp, amount=amount
        )
        self._apply(e)
        return e

    def close(self, timestamp: datetime) -> AccountEvent:
        self._ensure_open()
        if self._balance != Money.zero():
            raise NonZeroBalanceException(
                "Cannot close account with non-zero balance."
            )

        e = AccountClosed(account_id=self._account_id, timestamp=timestamp)
        self._apply(e)
        return e

    def transaction_history(self) -> list[Transaction]:
        transactions: list[Transaction] = []
        running = Money.zero()
        for e in self._events:
            if isinstance(e, MoneyDeposited):
                running = running + e.amount
                transactions.append(Transaction(e.timestamp, e.amount, running))
            elif isinstance(e, MoneyWithdrawn):
                running = running - e.amount
                transactions.append(Transaction(e.timestamp, -e.amount, running))
        return transactions

    def summary(self) -> AccountSummary:
        tx_count = sum(
            1 for e in self._events if isinstance(e, (MoneyDeposited, MoneyWithdrawn))
        )
        return AccountSummary(self._owner_name, self._balance, tx_count, self._status)

    def balance_at(self, point_in_time: datetime) -> Money:
        balance = Money.zero()
        for e in self._events:
            if e.timestamp > point_in_time:
                break
            if isinstance(e, MoneyDeposited):
                balance = balance + e.amount
            elif isinstance(e, MoneyWithdrawn):
                balance = balance - e.amount
        return balance

    def transactions_in_range(
        self, from_time: datetime, to_time: datetime
    ) -> list[Transaction]:
        transactions: list[Transaction] = []
        running = Money.zero()
        for e in self._events:
            if isinstance(e, MoneyDeposited):
                running = running + e.amount
                if from_time <= e.timestamp <= to_time:
                    transactions.append(Transaction(e.timestamp, e.amount, running))
            elif isinstance(e, MoneyWithdrawn):
                running = running - e.amount
                if from_time <= e.timestamp <= to_time:
                    transactions.append(Transaction(e.timestamp, -e.amount, running))
        return transactions

    def _apply(self, e: AccountEvent) -> None:
        if isinstance(e, AccountOpened):
            self._owner_name = e.owner_name
            self._opened = True
            self._status = AccountStatus.OPEN
        elif isinstance(e, MoneyDeposited):
            self._balance = self._balance + e.amount
        elif isinstance(e, MoneyWithdrawn):
            self._balance = self._balance - e.amount
        elif isinstance(e, AccountClosed):
            self._status = AccountStatus.CLOSED
        self._events.append(e)

    def _ensure_open(self) -> None:
        if not self._opened:
            raise AccountNotOpenException("Account was never opened.")
        if self._status == AccountStatus.CLOSED:
            raise AccountClosedException("Account is closed.")
