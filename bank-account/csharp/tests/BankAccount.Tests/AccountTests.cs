using FluentAssertions;
using Xunit;

namespace BankAccount.Tests;

public class AccountTests
{
    private static readonly DateOnly Jan15 = new(2026, 1, 15);
    private static readonly DateOnly Jan20 = new(2026, 1, 20);
    private static readonly DateOnly Jan25 = new(2026, 1, 25);

    [Fact]
    public void New_account_opens_with_a_zero_balance()
    {
        var (account, _) = new AccountBuilder().Build();

        account.Balance.Should().Be(Money.Zero);
    }

    [Fact]
    public void New_account_has_no_transactions()
    {
        var (account, _) = new AccountBuilder().Build();

        account.Transactions.Should().BeEmpty();
    }

    [Fact]
    public void Depositing_a_positive_amount_increases_the_balance()
    {
        var (account, _) = new AccountBuilder().Build();

        account.Deposit(new Money(500m));

        account.Balance.Should().Be(new Money(500m));
    }

    [Fact]
    public void Depositing_records_a_transaction_with_the_clock_date()
    {
        var (account, clock) = new AccountBuilder().OpenedOn(Jan15).Build();

        account.Deposit(new Money(500m));

        account.Transactions.Should().ContainSingle()
            .Which.Should().Be(new Transaction(Jan15, new Money(500m), new Money(500m)));
    }

    [Fact]
    public void Depositing_zero_is_rejected()
    {
        var (account, _) = new AccountBuilder().Build();

        var accepted = account.Deposit(Money.Zero);

        accepted.Should().BeFalse();
    }

    [Fact]
    public void Depositing_a_negative_amount_is_rejected()
    {
        var (account, _) = new AccountBuilder().Build();

        var accepted = account.Deposit(new Money(-50m));

        accepted.Should().BeFalse();
    }

    [Fact]
    public void Rejected_deposit_leaves_the_balance_unchanged()
    {
        var (account, _) = new AccountBuilder().WithDepositOn(Jan15, 600m).Build();

        account.Deposit(new Money(-50m));

        account.Balance.Should().Be(new Money(600m));
    }

    [Fact]
    public void Rejected_deposit_leaves_no_transaction_on_the_log()
    {
        var (account, _) = new AccountBuilder().WithDepositOn(Jan15, 600m).Build();
        var transactionsBefore = account.Transactions.Count;

        account.Deposit(new Money(0m));

        account.Transactions.Count.Should().Be(transactionsBefore);
    }

    [Fact]
    public void Withdrawing_a_positive_amount_decreases_the_balance()
    {
        var (account, _) = new AccountBuilder().WithDepositOn(Jan15, 500m).Build();

        account.Withdraw(new Money(100m));

        account.Balance.Should().Be(new Money(400m));
    }

    [Fact]
    public void Withdrawing_records_a_transaction_with_the_clock_date()
    {
        var (account, clock) = new AccountBuilder().WithDepositOn(Jan15, 500m).Build();
        clock.AdvanceTo(Jan20);

        account.Withdraw(new Money(100m));

        account.Transactions.Last()
            .Should().Be(new Transaction(Jan20, new Money(-100m), new Money(400m)));
    }

    [Fact]
    public void Withdrawing_zero_is_rejected()
    {
        var (account, _) = new AccountBuilder().WithDepositOn(Jan15, 500m).Build();

        var accepted = account.Withdraw(Money.Zero);

        accepted.Should().BeFalse();
    }

    [Fact]
    public void Withdrawing_a_negative_amount_is_rejected()
    {
        var (account, _) = new AccountBuilder().WithDepositOn(Jan15, 500m).Build();

        var accepted = account.Withdraw(new Money(-10m));

        accepted.Should().BeFalse();
    }

    [Fact]
    public void Withdrawing_more_than_the_balance_is_rejected_as_insufficient_funds()
    {
        var (account, _) = new AccountBuilder().WithDepositOn(Jan15, 600m).Build();

        var accepted = account.Withdraw(new Money(700m));

        accepted.Should().BeFalse();
    }

    [Fact]
    public void Rejected_withdrawal_leaves_the_balance_unchanged()
    {
        var (account, _) = new AccountBuilder().WithDepositOn(Jan15, 600m).Build();

        account.Withdraw(new Money(700m));

        account.Balance.Should().Be(new Money(600m));
    }

    [Fact]
    public void Rejected_withdrawal_leaves_no_transaction_on_the_log()
    {
        var (account, _) = new AccountBuilder().WithDepositOn(Jan15, 600m).Build();
        var transactionsBefore = account.Transactions.Count;

        account.Withdraw(new Money(700m));

        account.Transactions.Count.Should().Be(transactionsBefore);
    }

    [Fact]
    public void Statement_of_a_new_account_prints_only_the_header()
    {
        var (account, _) = new AccountBuilder().Build();

        account.PrintStatement().Should().Be("Date       | Amount  | Balance");
    }

    [Fact]
    public void Statement_lists_transactions_in_chronological_order()
    {
        var (account, clock) = new AccountBuilder()
            .WithDepositOn(Jan15, 500m)
            .WithDepositOn(Jan25, 200m)
            .Build();
        clock.AdvanceTo(Jan20);
        account.Withdraw(new Money(100m));

        var statement = account.PrintStatement();

        var lines = statement.Split('\n');
        lines[1].Should().Contain("2026-01-15");
        lines[2].Should().Contain("2026-01-25");
        lines[3].Should().Contain("2026-01-20");
    }

    [Fact]
    public void Statement_shows_running_balance_after_each_transaction()
    {
        var (account, clock) = new AccountBuilder().OpenedOn(Jan15).Build();
        account.Deposit(new Money(500m));
        clock.AdvanceTo(Jan20);
        account.Withdraw(new Money(100m));
        clock.AdvanceTo(Jan25);
        account.Deposit(new Money(200m));

        var statement = account.PrintStatement();

        statement.Should().Contain("500.00 |  500.00");
        statement.Should().Contain("-100.00 |  400.00");
        statement.Should().Contain("200.00 |  600.00");
    }

    [Fact]
    public void Statement_formats_amounts_with_two_decimal_places()
    {
        var (account, _) = new AccountBuilder().OpenedOn(Jan15).Build();
        account.Deposit(new Money(42.5m));

        account.PrintStatement().Should().Contain("42.50");
    }

    [Fact]
    public void Withdrawals_appear_as_negative_amounts_in_the_statement()
    {
        var (account, clock) = new AccountBuilder().WithDepositOn(Jan15, 500m).Build();
        clock.AdvanceTo(Jan20);
        account.Withdraw(new Money(100m));

        account.PrintStatement().Should().Contain("-100.00");
    }
}
