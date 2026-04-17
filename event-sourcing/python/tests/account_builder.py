from __future__ import annotations

from datetime import datetime, timedelta, timezone

from event_sourcing import (
    Account,
    AccountEvent,
    AccountOpened,
    MoneyDeposited,
    MoneyWithdrawn,
    AccountClosed,
    Money,
)

T0 = datetime(2026, 1, 1, tzinfo=timezone.utc)


class AccountBuilder:
    def __init__(self) -> None:
        self._account_id = "ACC-001"
        self._owner_name = "Alice"
        self._opened_at = T0
        self._additional_events: list[AccountEvent] = []
        self._time_step = 0

    def with_account_id(self, account_id: str) -> AccountBuilder:
        self._account_id = account_id
        return self

    def with_owner_name(self, name: str) -> AccountBuilder:
        self._owner_name = name
        return self

    def opened_at(self, timestamp: datetime) -> AccountBuilder:
        self._opened_at = timestamp
        return self

    def with_deposit(
        self, amount: float | int, timestamp: datetime | None = None
    ) -> AccountBuilder:
        self._additional_events.append(
            MoneyDeposited(
                account_id=self._account_id,
                timestamp=timestamp or self._next_timestamp(),
                amount=Money(amount),
            )
        )
        return self

    def with_withdrawal(
        self, amount: float | int, timestamp: datetime | None = None
    ) -> AccountBuilder:
        self._additional_events.append(
            MoneyWithdrawn(
                account_id=self._account_id,
                timestamp=timestamp or self._next_timestamp(),
                amount=Money(amount),
            )
        )
        return self

    def closed(self, timestamp: datetime | None = None) -> AccountBuilder:
        self._additional_events.append(
            AccountClosed(
                account_id=self._account_id,
                timestamp=timestamp or self._next_timestamp(),
            )
        )
        return self

    def build(self) -> Account:
        events: list[AccountEvent] = [
            AccountOpened(
                account_id=self._account_id,
                timestamp=self._opened_at,
                owner_name=self._owner_name,
            ),
            *self._additional_events,
        ]
        return Account.rebuild(events)

    def _next_timestamp(self) -> datetime:
        self._time_step += 1
        return self._opened_at + timedelta(hours=self._time_step)
