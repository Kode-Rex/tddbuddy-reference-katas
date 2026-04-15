using FluentAssertions;
using Xunit;

namespace GildedRose.Tests;

public class BackstagePassTests
{
    [Fact]
    public void Backstage_pass_quality_increases_by_one_when_concert_is_more_than_ten_days_away()
    {
        var item = new ItemBuilder().BackstagePass().WithQuality(10).WithSellIn(15).Build();
        var inn = new GildedRoseInn(new Inventory(new[] { item }));

        inn.UpdateInventory();

        item.Quality.Should().Be(11);
    }

    [Fact]
    public void Backstage_pass_quality_increases_by_two_when_concert_is_ten_days_or_fewer_away()
    {
        var item = new ItemBuilder().BackstagePass().WithQuality(10).WithSellIn(10).Build();
        var inn = new GildedRoseInn(new Inventory(new[] { item }));

        inn.UpdateInventory();

        item.Quality.Should().Be(12);
    }

    [Fact]
    public void Backstage_pass_quality_increases_by_three_when_concert_is_five_days_or_fewer_away()
    {
        var item = new ItemBuilder().BackstagePass().WithQuality(10).WithSellIn(5).Build();
        var inn = new GildedRoseInn(new Inventory(new[] { item }));

        inn.UpdateInventory();

        item.Quality.Should().Be(13);
    }

    [Fact]
    public void Backstage_pass_quality_drops_to_zero_after_the_concert()
    {
        var item = new ItemBuilder().BackstagePass().WithQuality(20).WithSellIn(0).Build();
        var inn = new GildedRoseInn(new Inventory(new[] { item }));

        inn.UpdateInventory();

        item.Quality.Should().Be(0);
    }
}
