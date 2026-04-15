namespace BankAccount.Tests;

public class AccountBuilder
{
    private DateOnly _openedOn = new(2026, 1, 1);
    private readonly List<(DateOnly Date, decimal Amount)> _seededDeposits = new();

    public AccountBuilder OpenedOn(DateOnly date) { _openedOn = date; return this; }

    public AccountBuilder WithDepositOn(DateOnly date, decimal amount)
    {
        _seededDeposits.Add((date, amount));
        return this;
    }

    public (Account Account, FixedClock Clock) Build()
    {
        var clock = new FixedClock(_openedOn);
        var account = new Account(clock);
        foreach (var (date, amount) in _seededDeposits)
        {
            clock.AdvanceTo(date);
            account.Deposit(new Money(amount));
        }
        return (account, clock);
    }
}
