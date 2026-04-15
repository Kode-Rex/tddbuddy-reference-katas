namespace RateLimiter;

public readonly record struct Rule
{
    public int MaxRequests { get; }
    public TimeSpan Window { get; }

    public Rule(int maxRequests, TimeSpan window)
    {
        if (maxRequests <= 0)
            throw new LimiterRuleInvalidException("Max requests must be positive");
        if (window <= TimeSpan.Zero)
            throw new LimiterRuleInvalidException("Window must be positive");

        MaxRequests = maxRequests;
        Window = window;
    }
}
