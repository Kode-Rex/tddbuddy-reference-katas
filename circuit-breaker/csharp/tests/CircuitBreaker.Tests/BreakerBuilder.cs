namespace CircuitBreaker.Tests;

public class BreakerBuilder
{
    private int _threshold = Breaker.DefaultFailureThreshold;
    private TimeSpan _timeout = Breaker.DefaultResetTimeout;
    private DateTime _start = new(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    private FixedClock? _clock;

    public BreakerBuilder WithThreshold(int threshold) { _threshold = threshold; return this; }

    public BreakerBuilder WithTimeout(TimeSpan timeout) { _timeout = timeout; return this; }

    public BreakerBuilder StartingAt(DateTime start) { _start = start; return this; }

    public BreakerBuilder WithClock(FixedClock clock) { _clock = clock; return this; }

    public (Breaker Breaker, FixedClock Clock) Build()
    {
        var clock = _clock ?? new FixedClock(_start);
        var breaker = new Breaker(_threshold, _timeout, clock);
        return (breaker, clock);
    }
}
