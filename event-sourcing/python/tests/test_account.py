from datetime import datetime, timezone

import pytest

from event_sourcing import (
    Account,
    Money,
    Transaction,
    AccountSummary,
    AccountStatus,
    AccountNotOpenException,
    AccountClosedException,
    InsufficientFundsException,
    InvalidAmountException,
    NonZeroBalanceException,
    MoneyDeposited,
    MoneyWithdrawn,
    AccountClosed,
)

from .account_builder import AccountBuilder
from .event_builder import a_money_deposited

T1 = datetime(2026, 1, 15, 10, 0, 0, tzinfo=timezone.utc)
T2 = datetime(2026, 1, 16, 10, 0, 0, tzinfo=timezone.utc)
T3 = datetime(2026, 1, 17, 10, 0, 0, tzinfo=timezone.utc)
T4 = datetime(2026, 1, 18, 10, 0, 0, tzinfo=timezone.utc)


# --- Replay — Balance ---


def test_replaying_an_opened_account_has_a_zero_balance():
    account = AccountBuilder().build()
    assert account.balance == Money.zero()


def test_replaying_a_single_deposit_yields_that_amount_as_the_balance():
    account = AccountBuilder().with_deposit(100).build()
    assert account.balance == Money(100)


def test_replaying_multiple_deposits_yields_their_sum():
    account = AccountBuilder().with_deposit(100).with_deposit(50).build()
    assert account.balance == Money(150)


def test_replaying_deposits_and_withdrawals_yields_the_net_balance():
    account = AccountBuilder().with_deposit(100).with_withdrawal(30).build()
    assert account.balance == Money(70)


def test_withdrawing_the_full_balance_yields_zero():
    account = AccountBuilder().with_deposit(100).with_withdrawal(100).build()
    assert account.balance == Money.zero()


# --- Command Validation — Deposits ---


def test_depositing_a_positive_amount_appends_a_money_deposited_event():
    account = AccountBuilder().build()
    result = account.deposit(Money(200), T1)
    assert isinstance(result, MoneyDeposited)
    assert account.balance == Money(200)


def test_depositing_zero_is_rejected():
    account = AccountBuilder().build()
    with pytest.raises(InvalidAmountException, match="Deposit amount must be positive."):
        account.deposit(Money.zero(), T1)


def test_depositing_a_negative_amount_is_rejected():
    account = AccountBuilder().build()
    with pytest.raises(InvalidAmountException, match="Deposit amount must be positive."):
        account.deposit(Money(-50), T1)


def test_depositing_into_a_closed_account_is_rejected():
    account = AccountBuilder().closed().build()
    with pytest.raises(AccountClosedException, match="Account is closed."):
        account.deposit(Money(100), T1)


# --- Command Validation — Withdrawals ---


def test_withdrawing_a_positive_amount_appends_a_money_withdrawn_event():
    account = AccountBuilder().with_deposit(500).build()
    result = account.withdraw(Money(100), T1)
    assert isinstance(result, MoneyWithdrawn)
    assert account.balance == Money(400)


def test_withdrawing_zero_is_rejected():
    account = AccountBuilder().with_deposit(500).build()
    with pytest.raises(
        InvalidAmountException, match="Withdrawal amount must be positive."
    ):
        account.withdraw(Money.zero(), T1)


def test_withdrawing_a_negative_amount_is_rejected():
    account = AccountBuilder().with_deposit(500).build()
    with pytest.raises(
        InvalidAmountException, match="Withdrawal amount must be positive."
    ):
        account.withdraw(Money(-10), T1)


def test_withdrawing_more_than_the_balance_is_rejected_as_insufficient_funds():
    account = AccountBuilder().with_deposit(100).build()
    with pytest.raises(
        InsufficientFundsException,
        match="Withdrawal amount exceeds current balance.",
    ):
        account.withdraw(Money(150), T1)


def test_withdrawing_from_a_closed_account_is_rejected():
    account = AccountBuilder().closed().build()
    with pytest.raises(AccountClosedException, match="Account is closed."):
        account.withdraw(Money(50), T1)


# --- Command Validation — Lifecycle ---


def test_operating_on_an_account_that_was_never_opened_is_rejected():
    with pytest.raises(
        AccountNotOpenException, match="Account was never opened."
    ):
        Account.rebuild([a_money_deposited(100)])


def test_closing_an_account_with_a_zero_balance_appends_an_account_closed_event():
    account = AccountBuilder().build()
    result = account.close(T1)
    assert isinstance(result, AccountClosed)
    assert account.status == AccountStatus.CLOSED


def test_closing_an_account_with_a_non_zero_balance_is_rejected():
    account = AccountBuilder().with_deposit(100).build()
    with pytest.raises(
        NonZeroBalanceException,
        match="Cannot close account with non-zero balance.",
    ):
        account.close(T1)


# --- Projection — Transaction History ---


def test_transaction_history_of_a_new_account_is_empty():
    account = AccountBuilder().build()
    assert account.transaction_history() == []


def test_transaction_history_lists_deposits_and_withdrawals_with_running_balances():
    account = (
        AccountBuilder()
        .with_deposit(100, T1)
        .with_deposit(50, T2)
        .with_withdrawal(30, T3)
        .build()
    )

    history = account.transaction_history()

    assert len(history) == 3
    assert history[0] == Transaction(T1, Money(100), Money(100))
    assert history[1] == Transaction(T2, Money(50), Money(150))
    assert history[2] == Transaction(T3, Money(-30), Money(120))


def test_withdrawals_appear_as_negative_amounts_in_the_transaction_history():
    account = (
        AccountBuilder().with_deposit(200, T1).with_withdrawal(75, T2).build()
    )
    history = account.transaction_history()
    assert history[1].amount == Money(-75)


# --- Projection — Account Summary ---


def test_account_summary_shows_owner_name_balance_transaction_count_and_open_status():
    account = (
        AccountBuilder()
        .with_owner_name("Alice")
        .with_deposit(100)
        .with_deposit(50)
        .build()
    )
    summary = account.summary()
    assert summary == AccountSummary("Alice", Money(150), 2, AccountStatus.OPEN)


def test_account_summary_reflects_closed_status_after_closing():
    account = AccountBuilder().with_owner_name("Bob").closed().build()
    summary = account.summary()
    assert summary.status == AccountStatus.CLOSED


# --- Temporal Queries ---


def test_balance_at_a_point_in_time_replays_only_events_up_to_that_timestamp():
    account = (
        AccountBuilder()
        .with_deposit(100, T1)
        .with_deposit(50, T2)
        .with_deposit(25, T3)
        .build()
    )
    assert account.balance_at(T2) == Money(150)


def test_transactions_in_a_date_range_returns_only_matching_entries():
    account = (
        AccountBuilder()
        .with_deposit(100, T1)
        .with_deposit(50, T2)
        .with_withdrawal(30, T3)
        .with_deposit(25, T4)
        .build()
    )

    result = account.transactions_in_range(T2, T3)

    assert len(result) == 2
    assert result[0].amount == Money(50)
    assert result[1].amount == Money(-30)
