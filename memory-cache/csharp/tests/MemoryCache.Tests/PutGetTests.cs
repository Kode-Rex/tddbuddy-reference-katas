using FluentAssertions;

namespace MemoryCache.Tests;

public class PutGetTests
{
    [Fact]
    public void A_new_cache_has_size_zero()
    {
        var (cache, _) = new CacheBuilder<string>().Build();

        cache.Size.Should().Be(0);
    }

    [Fact]
    public void A_new_cache_contains_no_keys()
    {
        var (cache, _) = new CacheBuilder<string>().Build();

        cache.Contains("anything").Should().BeFalse();
    }

    [Fact]
    public void Cache_rejects_non_positive_capacity_with_CacheCapacityInvalidException()
    {
        var act = () => new CacheBuilder<string>().WithCapacity(0).Build();

        act.Should().Throw<CacheCapacityInvalidException>()
            .WithMessage("Capacity must be positive");
    }

    [Fact]
    public void Cache_rejects_non_positive_ttl_with_CacheTtlInvalidException()
    {
        var act = () => new CacheBuilder<string>().WithTtl(TimeSpan.Zero).Build();

        act.Should().Throw<CacheTtlInvalidException>()
            .WithMessage("TTL must be positive");
    }

    [Fact]
    public void Putting_a_key_then_getting_it_returns_the_stored_value()
    {
        var (cache, _) = new CacheBuilder<string>().Build();

        cache.Put("alpha", "one");

        cache.Get("alpha").Should().Be("one");
    }

    [Fact]
    public void Getting_a_missing_key_returns_null()
    {
        var (cache, _) = new CacheBuilder<string>().Build();

        cache.Get("absent").Should().BeNull();
    }

    [Fact]
    public void Put_increases_the_size_by_one()
    {
        var (cache, _) = new CacheBuilder<string>().Build();

        cache.Put("alpha", "one");

        cache.Size.Should().Be(1);
    }

    [Fact]
    public void Putting_the_same_key_twice_replaces_the_value_without_growing_the_size()
    {
        var (cache, _) = new CacheBuilder<string>().Build();

        cache.Put("alpha", "one");
        cache.Put("alpha", "two");

        cache.Size.Should().Be(1);
        cache.Get("alpha").Should().Be("two");
    }

    [Fact]
    public void Contains_returns_true_for_a_stored_key()
    {
        var (cache, _) = new CacheBuilder<string>().Build();

        cache.Put("alpha", "one");

        cache.Contains("alpha").Should().BeTrue();
    }

    [Fact]
    public void Contains_returns_false_for_a_missing_key()
    {
        var (cache, _) = new CacheBuilder<string>().Build();

        cache.Contains("absent").Should().BeFalse();
    }
}
