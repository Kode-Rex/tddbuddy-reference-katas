namespace EventSourcing;

public class Account
{
    private readonly List<AccountEvent> _events = new();
    private string _ownerName = string.Empty;
    private Money _balance = Money.Zero;
    private AccountStatus _status = AccountStatus.Open;
    private bool _opened;

    public string AccountId { get; }

    public Money Balance => _balance;

    public AccountStatus Status => _status;

    private Account(string accountId)
    {
        AccountId = accountId;
    }

    public static Account Rebuild(IEnumerable<AccountEvent> events)
    {
        var eventList = events.ToList();
        if (eventList.Count == 0)
            throw new AccountNotOpenException("Account has no events.");

        var first = eventList[0];
        if (first is not AccountOpened)
            throw new AccountNotOpenException("Account was never opened.");

        var account = new Account(first.AccountId);
        foreach (var e in eventList)
        {
            account.Apply(e);
        }
        return account;
    }

    public AccountEvent Deposit(Money amount, DateTime timestamp)
    {
        EnsureOpen();
        if (!amount.IsPositive)
            throw new InvalidAmountException("Deposit amount must be positive.");

        var e = new MoneyDeposited(AccountId, amount, timestamp);
        Apply(e);
        return e;
    }

    public AccountEvent Withdraw(Money amount, DateTime timestamp)
    {
        EnsureOpen();
        if (!amount.IsPositive)
            throw new InvalidAmountException("Withdrawal amount must be positive.");
        if (amount > _balance)
            throw new InsufficientFundsException("Withdrawal amount exceeds current balance.");

        var e = new MoneyWithdrawn(AccountId, amount, timestamp);
        Apply(e);
        return e;
    }

    public AccountEvent Close(DateTime timestamp)
    {
        EnsureOpen();
        if (_balance != Money.Zero)
            throw new NonZeroBalanceException("Cannot close account with non-zero balance.");

        var e = new AccountClosed(AccountId, timestamp);
        Apply(e);
        return e;
    }

    public IReadOnlyList<Transaction> TransactionHistory()
    {
        var transactions = new List<Transaction>();
        var running = Money.Zero;
        foreach (var e in _events)
        {
            switch (e)
            {
                case MoneyDeposited d:
                    running += d.Amount;
                    transactions.Add(new Transaction(d.Timestamp, d.Amount, running));
                    break;
                case MoneyWithdrawn w:
                    running -= w.Amount;
                    transactions.Add(new Transaction(w.Timestamp, new Money(-w.Amount.Amount), running));
                    break;
            }
        }
        return transactions;
    }

    public AccountSummary Summary()
    {
        var txCount = _events.Count(e => e is MoneyDeposited or MoneyWithdrawn);
        return new AccountSummary(_ownerName, _balance, txCount, _status);
    }

    public Money BalanceAt(DateTime pointInTime)
    {
        var balance = Money.Zero;
        foreach (var e in _events)
        {
            if (e.Timestamp > pointInTime) break;
            switch (e)
            {
                case MoneyDeposited d:
                    balance += d.Amount;
                    break;
                case MoneyWithdrawn w:
                    balance -= w.Amount;
                    break;
            }
        }
        return balance;
    }

    public IReadOnlyList<Transaction> TransactionsInRange(DateTime from, DateTime to)
    {
        var transactions = new List<Transaction>();
        var running = Money.Zero;
        foreach (var e in _events)
        {
            switch (e)
            {
                case MoneyDeposited d:
                    running += d.Amount;
                    if (d.Timestamp >= from && d.Timestamp <= to)
                        transactions.Add(new Transaction(d.Timestamp, d.Amount, running));
                    break;
                case MoneyWithdrawn w:
                    running -= w.Amount;
                    if (w.Timestamp >= from && w.Timestamp <= to)
                        transactions.Add(new Transaction(w.Timestamp, new Money(-w.Amount.Amount), running));
                    break;
            }
        }
        return transactions;
    }

    private void Apply(AccountEvent e)
    {
        switch (e)
        {
            case AccountOpened opened:
                _ownerName = opened.OwnerName;
                _opened = true;
                _status = AccountStatus.Open;
                break;
            case MoneyDeposited deposited:
                _balance += deposited.Amount;
                break;
            case MoneyWithdrawn withdrawn:
                _balance -= withdrawn.Amount;
                break;
            case AccountClosed:
                _status = AccountStatus.Closed;
                break;
        }
        _events.Add(e);
    }

    private void EnsureOpen()
    {
        if (!_opened)
            throw new AccountNotOpenException("Account was never opened.");
        if (_status == AccountStatus.Closed)
            throw new AccountClosedException("Account is closed.");
    }
}
