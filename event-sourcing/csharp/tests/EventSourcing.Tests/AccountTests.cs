using FluentAssertions;
using Xunit;

namespace EventSourcing.Tests;

public class AccountTests
{
    private static readonly DateTime T1 = new(2026, 1, 15, 10, 0, 0, DateTimeKind.Utc);
    private static readonly DateTime T2 = new(2026, 1, 16, 10, 0, 0, DateTimeKind.Utc);
    private static readonly DateTime T3 = new(2026, 1, 17, 10, 0, 0, DateTimeKind.Utc);
    private static readonly DateTime T4 = new(2026, 1, 18, 10, 0, 0, DateTimeKind.Utc);

    // --- Replay — Balance ---

    [Fact]
    public void Replaying_an_opened_account_has_a_zero_balance()
    {
        var account = new AccountBuilder().Build();

        account.Balance.Should().Be(Money.Zero);
    }

    [Fact]
    public void Replaying_a_single_deposit_yields_that_amount_as_the_balance()
    {
        var account = new AccountBuilder()
            .WithDeposit(100m)
            .Build();

        account.Balance.Should().Be(new Money(100m));
    }

    [Fact]
    public void Replaying_multiple_deposits_yields_their_sum()
    {
        var account = new AccountBuilder()
            .WithDeposit(100m)
            .WithDeposit(50m)
            .Build();

        account.Balance.Should().Be(new Money(150m));
    }

    [Fact]
    public void Replaying_deposits_and_withdrawals_yields_the_net_balance()
    {
        var account = new AccountBuilder()
            .WithDeposit(100m)
            .WithWithdrawal(30m)
            .Build();

        account.Balance.Should().Be(new Money(70m));
    }

    [Fact]
    public void Withdrawing_the_full_balance_yields_zero()
    {
        var account = new AccountBuilder()
            .WithDeposit(100m)
            .WithWithdrawal(100m)
            .Build();

        account.Balance.Should().Be(Money.Zero);
    }

    // --- Command Validation — Deposits ---

    [Fact]
    public void Depositing_a_positive_amount_appends_a_MoneyDeposited_event()
    {
        var account = new AccountBuilder().Build();

        var result = account.Deposit(new Money(200m), T1);

        result.Should().BeOfType<MoneyDeposited>();
        account.Balance.Should().Be(new Money(200m));
    }

    [Fact]
    public void Depositing_zero_is_rejected()
    {
        var account = new AccountBuilder().Build();

        var act = () => account.Deposit(Money.Zero, T1);

        act.Should().Throw<InvalidAmountException>()
            .WithMessage("Deposit amount must be positive.");
    }

    [Fact]
    public void Depositing_a_negative_amount_is_rejected()
    {
        var account = new AccountBuilder().Build();

        var act = () => account.Deposit(new Money(-50m), T1);

        act.Should().Throw<InvalidAmountException>()
            .WithMessage("Deposit amount must be positive.");
    }

    [Fact]
    public void Depositing_into_a_closed_account_is_rejected()
    {
        var account = new AccountBuilder().Closed().Build();

        var act = () => account.Deposit(new Money(100m), T1);

        act.Should().Throw<AccountClosedException>()
            .WithMessage("Account is closed.");
    }

    // --- Command Validation — Withdrawals ---

    [Fact]
    public void Withdrawing_a_positive_amount_appends_a_MoneyWithdrawn_event()
    {
        var account = new AccountBuilder()
            .WithDeposit(500m)
            .Build();

        var result = account.Withdraw(new Money(100m), T1);

        result.Should().BeOfType<MoneyWithdrawn>();
        account.Balance.Should().Be(new Money(400m));
    }

    [Fact]
    public void Withdrawing_zero_is_rejected()
    {
        var account = new AccountBuilder()
            .WithDeposit(500m)
            .Build();

        var act = () => account.Withdraw(Money.Zero, T1);

        act.Should().Throw<InvalidAmountException>()
            .WithMessage("Withdrawal amount must be positive.");
    }

    [Fact]
    public void Withdrawing_a_negative_amount_is_rejected()
    {
        var account = new AccountBuilder()
            .WithDeposit(500m)
            .Build();

        var act = () => account.Withdraw(new Money(-10m), T1);

        act.Should().Throw<InvalidAmountException>()
            .WithMessage("Withdrawal amount must be positive.");
    }

    [Fact]
    public void Withdrawing_more_than_the_balance_is_rejected_as_insufficient_funds()
    {
        var account = new AccountBuilder()
            .WithDeposit(100m)
            .Build();

        var act = () => account.Withdraw(new Money(150m), T1);

        act.Should().Throw<InsufficientFundsException>()
            .WithMessage("Withdrawal amount exceeds current balance.");
    }

    [Fact]
    public void Withdrawing_from_a_closed_account_is_rejected()
    {
        var account = new AccountBuilder().Closed().Build();

        var act = () => account.Withdraw(new Money(50m), T1);

        act.Should().Throw<AccountClosedException>()
            .WithMessage("Account is closed.");
    }

    // --- Command Validation — Lifecycle ---

    [Fact]
    public void Operating_on_an_account_that_was_never_opened_is_rejected()
    {
        var act = () => Account.Rebuild(new[]
        {
            EventBuilder.AMoneyDeposited(100m)
        });

        act.Should().Throw<AccountNotOpenException>()
            .WithMessage("Account was never opened.");
    }

    [Fact]
    public void Closing_an_account_with_a_zero_balance_appends_an_AccountClosed_event()
    {
        var account = new AccountBuilder().Build();

        var result = account.Close(T1);

        result.Should().BeOfType<AccountClosed>();
        account.Status.Should().Be(AccountStatus.Closed);
    }

    [Fact]
    public void Closing_an_account_with_a_non_zero_balance_is_rejected()
    {
        var account = new AccountBuilder()
            .WithDeposit(100m)
            .Build();

        var act = () => account.Close(T1);

        act.Should().Throw<NonZeroBalanceException>()
            .WithMessage("Cannot close account with non-zero balance.");
    }

    // --- Projection — Transaction History ---

    [Fact]
    public void Transaction_history_of_a_new_account_is_empty()
    {
        var account = new AccountBuilder().Build();

        account.TransactionHistory().Should().BeEmpty();
    }

    [Fact]
    public void Transaction_history_lists_deposits_and_withdrawals_with_running_balances()
    {
        var account = new AccountBuilder()
            .WithDeposit(100m, T1)
            .WithDeposit(50m, T2)
            .WithWithdrawal(30m, T3)
            .Build();

        var history = account.TransactionHistory();

        history.Should().HaveCount(3);
        history[0].Should().Be(new Transaction(T1, new Money(100m), new Money(100m)));
        history[1].Should().Be(new Transaction(T2, new Money(50m), new Money(150m)));
        history[2].Should().Be(new Transaction(T3, new Money(-30m), new Money(120m)));
    }

    [Fact]
    public void Withdrawals_appear_as_negative_amounts_in_the_transaction_history()
    {
        var account = new AccountBuilder()
            .WithDeposit(200m, T1)
            .WithWithdrawal(75m, T2)
            .Build();

        var history = account.TransactionHistory();

        history[1].Amount.Should().Be(new Money(-75m));
    }

    // --- Projection — Account Summary ---

    [Fact]
    public void Account_summary_shows_owner_name_balance_transaction_count_and_open_status()
    {
        var account = new AccountBuilder()
            .WithOwnerName("Alice")
            .WithDeposit(100m)
            .WithDeposit(50m)
            .Build();

        var summary = account.Summary();

        summary.Should().Be(new AccountSummary("Alice", new Money(150m), 2, AccountStatus.Open));
    }

    [Fact]
    public void Account_summary_reflects_closed_status_after_closing()
    {
        var account = new AccountBuilder()
            .WithOwnerName("Bob")
            .Closed()
            .Build();

        var summary = account.Summary();

        summary.Status.Should().Be(AccountStatus.Closed);
    }

    // --- Temporal Queries ---

    [Fact]
    public void Balance_at_a_point_in_time_replays_only_events_up_to_that_timestamp()
    {
        var account = new AccountBuilder()
            .WithDeposit(100m, T1)
            .WithDeposit(50m, T2)
            .WithDeposit(25m, T3)
            .Build();

        account.BalanceAt(T2).Should().Be(new Money(150m));
    }

    [Fact]
    public void Transactions_in_a_date_range_returns_only_matching_entries()
    {
        var account = new AccountBuilder()
            .WithDeposit(100m, T1)
            .WithDeposit(50m, T2)
            .WithWithdrawal(30m, T3)
            .WithDeposit(25m, T4)
            .Build();

        var result = account.TransactionsInRange(T2, T3);

        result.Should().HaveCount(2);
        result[0].Amount.Should().Be(new Money(50m));
        result[1].Amount.Should().Be(new Money(-30m));
    }
}
