using FluentAssertions;

namespace MemoryCache.Tests;

public class TtlExpiryTests
{
    private static readonly TimeSpan OneMinute = TimeSpan.FromMinutes(1);

    [Fact]
    public void A_get_after_ttl_has_elapsed_returns_null()
    {
        var (cache, clock) = new CacheBuilder<string>().WithTtl(OneMinute).Build();
        cache.Put("alpha", "one");

        clock.Advance(OneMinute);

        cache.Get("alpha").Should().BeNull();
    }

    [Fact]
    public void Contains_returns_false_once_ttl_has_elapsed()
    {
        var (cache, clock) = new CacheBuilder<string>().WithTtl(OneMinute).Build();
        cache.Put("alpha", "one");

        clock.Advance(OneMinute);

        cache.Contains("alpha").Should().BeFalse();
    }

    [Fact]
    public void An_expired_entry_is_not_counted_in_size_after_eviction_sweep()
    {
        var (cache, clock) = new CacheBuilder<string>().WithTtl(OneMinute).Build();
        cache.Put("alpha", "one");
        cache.Put("beta", "two");

        clock.Advance(OneMinute);
        cache.EvictExpired();

        cache.Size.Should().Be(0);
    }

    [Fact]
    public void Explicit_evictExpired_removes_all_expired_entries()
    {
        var (cache, clock) = new CacheBuilder<string>().WithTtl(OneMinute).Build();
        cache.Put("alpha", "one");
        cache.Put("beta", "two");
        cache.Put("gamma", "three");

        clock.Advance(OneMinute);
        cache.EvictExpired();

        cache.Contains("alpha").Should().BeFalse();
        cache.Contains("beta").Should().BeFalse();
        cache.Contains("gamma").Should().BeFalse();
    }

    [Fact]
    public void Explicit_evictExpired_leaves_live_entries_intact()
    {
        var (cache, clock) = new CacheBuilder<string>().WithTtl(OneMinute).Build();
        cache.Put("old", "stale");
        clock.Advance(TimeSpan.FromSeconds(30));
        cache.Put("fresh", "alive");

        clock.Advance(TimeSpan.FromSeconds(30));
        cache.EvictExpired();

        cache.Contains("old").Should().BeFalse();
        cache.Contains("fresh").Should().BeTrue();
        cache.Get("fresh").Should().Be("alive");
    }

    [Fact]
    public void Ttl_is_measured_from_insertion_time_not_from_last_access()
    {
        var (cache, clock) = new CacheBuilder<string>().WithTtl(OneMinute).Build();
        cache.Put("alpha", "one");

        clock.Advance(TimeSpan.FromSeconds(30));
        cache.Get("alpha").Should().Be("one");

        clock.Advance(TimeSpan.FromSeconds(30));
        cache.Get("alpha").Should().BeNull();
    }
}
