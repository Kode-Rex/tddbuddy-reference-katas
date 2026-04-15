namespace BankAccount;

public readonly record struct Transaction(DateOnly Date, Money Amount, Money BalanceAfter);
