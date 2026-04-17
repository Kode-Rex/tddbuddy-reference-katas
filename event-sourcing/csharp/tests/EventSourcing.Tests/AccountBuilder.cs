namespace EventSourcing.Tests;

public class AccountBuilder
{
    private static readonly DateTime T0 = new(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    private string _accountId = "ACC-001";
    private string _ownerName = "Alice";
    private DateTime _openedAt = T0;
    private readonly List<AccountEvent> _additionalEvents = new();
    private int _timeStep;

    public AccountBuilder WithAccountId(string accountId) { _accountId = accountId; return this; }
    public AccountBuilder WithOwnerName(string name) { _ownerName = name; return this; }
    public AccountBuilder OpenedAt(DateTime timestamp) { _openedAt = timestamp; return this; }

    public AccountBuilder WithDeposit(decimal amount, DateTime? timestamp = null)
    {
        _additionalEvents.Add(new MoneyDeposited(_accountId, new Money(amount), timestamp ?? NextTimestamp()));
        return this;
    }

    public AccountBuilder WithWithdrawal(decimal amount, DateTime? timestamp = null)
    {
        _additionalEvents.Add(new MoneyWithdrawn(_accountId, new Money(amount), timestamp ?? NextTimestamp()));
        return this;
    }

    public AccountBuilder Closed(DateTime? timestamp = null)
    {
        _additionalEvents.Add(new AccountClosed(_accountId, timestamp ?? NextTimestamp()));
        return this;
    }

    public Account Build()
    {
        var events = new List<AccountEvent>
        {
            new AccountOpened(_accountId, _ownerName, _openedAt)
        };
        events.AddRange(_additionalEvents);
        return Account.Rebuild(events);
    }

    private DateTime NextTimestamp()
    {
        _timeStep++;
        return _openedAt.AddHours(_timeStep);
    }
}
