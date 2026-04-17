namespace EventSourcing;

public readonly record struct Transaction(DateTime Timestamp, Money Amount, Money BalanceAfter);
