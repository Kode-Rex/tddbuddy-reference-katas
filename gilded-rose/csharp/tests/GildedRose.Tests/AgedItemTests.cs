using FluentAssertions;
using Xunit;

namespace GildedRose.Tests;

public class AgedItemTests
{
    [Fact]
    public void Aged_items_gain_one_quality_per_day_while_fresh()
    {
        var item = new ItemBuilder().Aged().WithQuality(10).WithSellIn(5).Build();
        var inn = new GildedRoseInn(new Inventory(new[] { item }));

        inn.UpdateInventory();

        item.Quality.Should().Be(11);
    }

    [Fact]
    public void Aged_items_gain_two_quality_per_day_after_the_sell_by_date()
    {
        var item = new ItemBuilder().Aged().WithQuality(10).WithSellIn(0).Build();
        var inn = new GildedRoseInn(new Inventory(new[] { item }));

        inn.UpdateInventory();

        item.Quality.Should().Be(12);
    }

    [Fact]
    public void Aged_item_quality_never_exceeds_fifty()
    {
        var item = new ItemBuilder().Aged().WithQuality(50).WithSellIn(5).Build();
        var inn = new GildedRoseInn(new Inventory(new[] { item }));

        inn.UpdateInventory();

        item.Quality.Should().Be(50);
    }
}
