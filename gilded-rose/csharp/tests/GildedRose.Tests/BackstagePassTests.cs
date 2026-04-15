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
}
