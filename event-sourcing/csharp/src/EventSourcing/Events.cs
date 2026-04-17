namespace EventSourcing;

public abstract record AccountEvent(string AccountId, DateTime Timestamp);

public record AccountOpened(string AccountId, string OwnerName, DateTime Timestamp)
    : AccountEvent(AccountId, Timestamp);

public record MoneyDeposited(string AccountId, Money Amount, DateTime Timestamp)
    : AccountEvent(AccountId, Timestamp);

public record MoneyWithdrawn(string AccountId, Money Amount, DateTime Timestamp)
    : AccountEvent(AccountId, Timestamp);

public record AccountClosed(string AccountId, DateTime Timestamp)
    : AccountEvent(AccountId, Timestamp);
