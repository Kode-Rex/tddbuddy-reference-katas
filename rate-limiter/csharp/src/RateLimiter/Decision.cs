namespace RateLimiter;

public abstract record Decision
{
    public sealed record Allowed : Decision;

    public sealed record Rejected(TimeSpan RetryAfter) : Decision;
}
