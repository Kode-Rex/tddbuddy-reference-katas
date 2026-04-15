using FluentAssertions;
using Xunit;

namespace GildedRose.Tests;

public class LegendaryItemTests
{
    [Fact]
    public void Legendary_items_never_lose_quality()
    {
        var item = new ItemBuilder().Legendary().Named("Sulfuras, Hand of Ragnaros").WithQuality(80).WithSellIn(5).Build();
        var inn = new GildedRoseInn(new Inventory(new[] { item }));

        inn.UpdateInventory();

        item.Quality.Should().Be(80);
    }

    [Fact]
    public void Legendary_item_sell_in_never_changes()
    {
        var item = new ItemBuilder().Legendary().Named("Sulfuras, Hand of Ragnaros").WithQuality(80).WithSellIn(5).Build();
        var inn = new GildedRoseInn(new Inventory(new[] { item }));

        inn.UpdateInventory();

        item.SellIn.Should().Be(5);
    }
}
