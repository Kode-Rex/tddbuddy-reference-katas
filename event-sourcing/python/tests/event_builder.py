from __future__ import annotations

from datetime import datetime, timezone

from event_sourcing import (
    AccountOpened,
    MoneyDeposited,
    MoneyWithdrawn,
    AccountClosed,
    Money,
)

DEFAULT_ACCOUNT_ID = "ACC-001"
DEFAULT_OWNER_NAME = "Alice"
DEFAULT_TIMESTAMP = datetime(2026, 1, 1, tzinfo=timezone.utc)


def an_account_opened(
    account_id: str = DEFAULT_ACCOUNT_ID,
    owner_name: str = DEFAULT_OWNER_NAME,
    timestamp: datetime = DEFAULT_TIMESTAMP,
) -> AccountOpened:
    return AccountOpened(
        account_id=account_id, timestamp=timestamp, owner_name=owner_name
    )


def a_money_deposited(
    amount: float | int,
    account_id: str = DEFAULT_ACCOUNT_ID,
    timestamp: datetime = DEFAULT_TIMESTAMP,
) -> MoneyDeposited:
    return MoneyDeposited(
        account_id=account_id, timestamp=timestamp, amount=Money(amount)
    )


def a_money_withdrawn(
    amount: float | int,
    account_id: str = DEFAULT_ACCOUNT_ID,
    timestamp: datetime = DEFAULT_TIMESTAMP,
) -> MoneyWithdrawn:
    return MoneyWithdrawn(
        account_id=account_id, timestamp=timestamp, amount=Money(amount)
    )


def an_account_closed(
    account_id: str = DEFAULT_ACCOUNT_ID,
    timestamp: datetime = DEFAULT_TIMESTAMP,
) -> AccountClosed:
    return AccountClosed(account_id=account_id, timestamp=timestamp)
