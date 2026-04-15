using FluentAssertions;

namespace MemoryCache.Tests;

public class CapacityEvictionTests
{
    [Fact]
    public void Filling_the_cache_to_capacity_does_not_evict()
    {
        var (cache, _) = new CacheBuilder<string>().WithCapacity(3).Build();

        cache.Put("a", "1");
        cache.Put("b", "2");
        cache.Put("c", "3");

        cache.Size.Should().Be(3);
        cache.Contains("a").Should().BeTrue();
        cache.Contains("b").Should().BeTrue();
        cache.Contains("c").Should().BeTrue();
    }

    [Fact]
    public void Putting_a_new_key_when_at_capacity_evicts_the_least_recently_used_key()
    {
        var (cache, _) = new CacheBuilder<string>().WithCapacity(3).Build();
        cache.Put("a", "1");
        cache.Put("b", "2");
        cache.Put("c", "3");

        cache.Put("d", "4");

        cache.Size.Should().Be(3);
        cache.Contains("a").Should().BeFalse();
        cache.Contains("d").Should().BeTrue();
    }

    [Fact]
    public void Getting_a_key_refreshes_recency_so_it_is_not_the_next_evicted()
    {
        var (cache, _) = new CacheBuilder<string>().WithCapacity(3).Build();
        cache.Put("a", "1");
        cache.Put("b", "2");
        cache.Put("c", "3");

        cache.Get("a");
        cache.Put("d", "4");

        cache.Contains("a").Should().BeTrue();
        cache.Contains("b").Should().BeFalse();
    }

    [Fact]
    public void Replacing_an_existing_key_refreshes_recency_so_it_is_not_the_next_evicted()
    {
        var (cache, _) = new CacheBuilder<string>().WithCapacity(3).Build();
        cache.Put("a", "1");
        cache.Put("b", "2");
        cache.Put("c", "3");

        cache.Put("a", "1-new");
        cache.Put("d", "4");

        cache.Contains("a").Should().BeTrue();
        cache.Get("a").Should().Be("1-new");
        cache.Contains("b").Should().BeFalse();
    }
}
