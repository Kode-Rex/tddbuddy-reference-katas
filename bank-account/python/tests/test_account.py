from datetime import date

from bank_account import Money, Transaction

from .account_builder import AccountBuilder

JAN_15 = date(2026, 1, 15)
JAN_20 = date(2026, 1, 20)
JAN_25 = date(2026, 1, 25)


def test_new_account_opens_with_a_zero_balance():
    account, _ = AccountBuilder().build()
    assert account.balance == Money.zero()


def test_new_account_has_no_transactions():
    account, _ = AccountBuilder().build()
    assert account.transactions == []


def test_depositing_a_positive_amount_increases_the_balance():
    account, _ = AccountBuilder().build()
    account.deposit(Money(500))
    assert account.balance == Money(500)


def test_depositing_records_a_transaction_with_the_clock_date():
    account, _ = AccountBuilder().opened_on(JAN_15).build()
    account.deposit(Money(500))
    assert account.transactions == [Transaction(JAN_15, Money(500), Money(500))]


def test_depositing_zero_is_rejected():
    account, _ = AccountBuilder().build()
    assert account.deposit(Money.zero()) is False


def test_depositing_a_negative_amount_is_rejected():
    account, _ = AccountBuilder().build()
    assert account.deposit(Money(-50)) is False


def test_rejected_deposit_leaves_the_balance_unchanged():
    account, _ = AccountBuilder().with_deposit_on(JAN_15, 600).build()
    account.deposit(Money(-50))
    assert account.balance == Money(600)


def test_rejected_deposit_leaves_no_transaction_on_the_log():
    account, _ = AccountBuilder().with_deposit_on(JAN_15, 600).build()
    before = len(account.transactions)
    account.deposit(Money.zero())
    assert len(account.transactions) == before


def test_withdrawing_a_positive_amount_decreases_the_balance():
    account, _ = AccountBuilder().with_deposit_on(JAN_15, 500).build()
    account.withdraw(Money(100))
    assert account.balance == Money(400)


def test_withdrawing_records_a_transaction_with_the_clock_date():
    account, clock = AccountBuilder().with_deposit_on(JAN_15, 500).build()
    clock.advance_to(JAN_20)
    account.withdraw(Money(100))
    assert account.transactions[-1] == Transaction(JAN_20, Money(-100), Money(400))


def test_withdrawing_zero_is_rejected():
    account, _ = AccountBuilder().with_deposit_on(JAN_15, 500).build()
    assert account.withdraw(Money.zero()) is False


def test_withdrawing_a_negative_amount_is_rejected():
    account, _ = AccountBuilder().with_deposit_on(JAN_15, 500).build()
    assert account.withdraw(Money(-10)) is False


def test_withdrawing_more_than_the_balance_is_rejected_as_insufficient_funds():
    account, _ = AccountBuilder().with_deposit_on(JAN_15, 600).build()
    assert account.withdraw(Money(700)) is False


def test_rejected_withdrawal_leaves_the_balance_unchanged():
    account, _ = AccountBuilder().with_deposit_on(JAN_15, 600).build()
    account.withdraw(Money(700))
    assert account.balance == Money(600)


def test_rejected_withdrawal_leaves_no_transaction_on_the_log():
    account, _ = AccountBuilder().with_deposit_on(JAN_15, 600).build()
    before = len(account.transactions)
    account.withdraw(Money(700))
    assert len(account.transactions) == before


def test_statement_of_a_new_account_prints_only_the_header():
    account, _ = AccountBuilder().build()
    assert account.print_statement() == "Date       | Amount  | Balance"


def test_statement_lists_transactions_in_chronological_order():
    account, clock = (
        AccountBuilder()
        .with_deposit_on(JAN_15, 500)
        .with_deposit_on(JAN_25, 200)
        .build()
    )
    clock.advance_to(JAN_20)
    account.withdraw(Money(100))

    lines = account.print_statement().split("\n")
    assert "2026-01-15" in lines[1]
    assert "2026-01-25" in lines[2]
    assert "2026-01-20" in lines[3]


def test_statement_shows_running_balance_after_each_transaction():
    account, clock = AccountBuilder().opened_on(JAN_15).build()
    account.deposit(Money(500))
    clock.advance_to(JAN_20)
    account.withdraw(Money(100))
    clock.advance_to(JAN_25)
    account.deposit(Money(200))

    statement = account.print_statement()
    assert "500.00 |  500.00" in statement
    assert "-100.00 |  400.00" in statement
    assert "200.00 |  600.00" in statement


def test_statement_formats_amounts_with_two_decimal_places():
    account, _ = AccountBuilder().opened_on(JAN_15).build()
    account.deposit(Money("42.5"))
    assert "42.50" in account.print_statement()


def test_withdrawals_appear_as_negative_amounts_in_the_statement():
    account, clock = AccountBuilder().with_deposit_on(JAN_15, 500).build()
    clock.advance_to(JAN_20)
    account.withdraw(Money(100))
    assert "-100.00" in account.print_statement()
