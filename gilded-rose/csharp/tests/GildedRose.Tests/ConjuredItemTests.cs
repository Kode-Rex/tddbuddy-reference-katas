using FluentAssertions;
using Xunit;

namespace GildedRose.Tests;

public class ConjuredItemTests
{
    [Fact]
    public void Conjured_items_lose_two_quality_per_day_while_fresh()
    {
        var item = new ItemBuilder()
            .Conjured()
            .WithQuality(10)
            .WithSellIn(5)
            .Build();
        var inn = new GildedRoseInn(new Inventory(new[] { item }));

        inn.UpdateInventory();

        item.Quality.Should().Be(8);
    }

    [Fact]
    public void Conjured_items_lose_four_quality_per_day_after_the_sell_by_date()
    {
        var item = new ItemBuilder()
            .Conjured()
            .WithQuality(10)
            .WithSellIn(0)
            .Build();
        var inn = new GildedRoseInn(new Inventory(new[] { item }));

        inn.UpdateInventory();

        item.Quality.Should().Be(6);
    }

    [Fact]
    public void Conjured_item_quality_never_goes_below_zero()
    {
        var item = new ItemBuilder()
            .Conjured()
            .WithQuality(1)
            .WithSellIn(5)
            .Build();
        var inn = new GildedRoseInn(new Inventory(new[] { item }));

        inn.UpdateInventory();

        item.Quality.Should().Be(0);
    }
}
