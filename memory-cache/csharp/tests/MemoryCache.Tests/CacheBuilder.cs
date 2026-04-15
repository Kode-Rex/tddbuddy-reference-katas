namespace MemoryCache.Tests;

public class CacheBuilder<TValue>
{
    private int _capacity = Cache<TValue>.DefaultCapacity;
    private TimeSpan _ttl = Cache<TValue>.DefaultTtl;
    private DateTime _start = new(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    private FixedClock? _clock;

    public CacheBuilder<TValue> WithCapacity(int capacity) { _capacity = capacity; return this; }

    public CacheBuilder<TValue> WithTtl(TimeSpan ttl) { _ttl = ttl; return this; }

    public CacheBuilder<TValue> StartingAt(DateTime start) { _start = start; return this; }

    public CacheBuilder<TValue> WithClock(FixedClock clock) { _clock = clock; return this; }

    public (Cache<TValue> Cache, FixedClock Clock) Build()
    {
        var clock = _clock ?? new FixedClock(_start);
        var cache = new Cache<TValue>(_capacity, _ttl, clock);
        return (cache, clock);
    }
}
