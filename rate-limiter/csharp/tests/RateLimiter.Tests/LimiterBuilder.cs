namespace RateLimiter.Tests;

public class LimiterBuilder
{
    private int _maxRequests = Limiter.DefaultMaxRequests;
    private TimeSpan _window = Limiter.DefaultWindowDuration;
    private DateTime _start = new(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    private FixedClock? _clock;

    public LimiterBuilder WithMaxRequests(int maxRequests) { _maxRequests = maxRequests; return this; }

    public LimiterBuilder WithWindow(TimeSpan window) { _window = window; return this; }

    public LimiterBuilder StartingAt(DateTime start) { _start = start; return this; }

    public LimiterBuilder WithClock(FixedClock clock) { _clock = clock; return this; }

    public (Limiter Limiter, FixedClock Clock) Build()
    {
        var clock = _clock ?? new FixedClock(_start);
        var limiter = new Limiter(new Rule(_maxRequests, _window), clock);
        return (limiter, clock);
    }
}
