namespace EventSourcing;

public readonly record struct AccountSummary(
    string OwnerName,
    Money Balance,
    int TransactionCount,
    AccountStatus Status);
