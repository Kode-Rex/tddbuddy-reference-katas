namespace EventSourcing.Tests;

public static class EventBuilder
{
    private static readonly string DefaultAccountId = "ACC-001";
    private static readonly string DefaultOwnerName = "Alice";
    private static readonly DateTime DefaultTimestamp = new(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    public static AccountOpened AnAccountOpened(
        string? accountId = null,
        string? ownerName = null,
        DateTime? timestamp = null) =>
        new(accountId ?? DefaultAccountId, ownerName ?? DefaultOwnerName, timestamp ?? DefaultTimestamp);

    public static MoneyDeposited AMoneyDeposited(
        decimal amount,
        string? accountId = null,
        DateTime? timestamp = null) =>
        new(accountId ?? DefaultAccountId, new Money(amount), timestamp ?? DefaultTimestamp);

    public static MoneyWithdrawn AMoneyWithdrawn(
        decimal amount,
        string? accountId = null,
        DateTime? timestamp = null) =>
        new(accountId ?? DefaultAccountId, new Money(amount), timestamp ?? DefaultTimestamp);

    public static AccountClosed AnAccountClosed(
        string? accountId = null,
        DateTime? timestamp = null) =>
        new(accountId ?? DefaultAccountId, timestamp ?? DefaultTimestamp);
}
