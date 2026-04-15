using FluentAssertions;
using Xunit;

namespace GildedRose.Tests;

public class StandardItemTests
{
    [Fact]
    public void Standard_items_lose_one_quality_per_day_while_fresh()
    {
        var item = new ItemBuilder().Standard().WithQuality(10).WithSellIn(5).Build();
        var inn = new GildedRoseInn(new Inventory(new[] { item }));

        inn.UpdateInventory();

        item.Quality.Should().Be(9);
    }

    [Fact]
    public void Standard_items_lose_two_quality_per_day_after_the_sell_by_date()
    {
        var item = new ItemBuilder().Standard().WithQuality(10).WithSellIn(0).Build();
        var inn = new GildedRoseInn(new Inventory(new[] { item }));

        inn.UpdateInventory();

        item.Quality.Should().Be(8);
    }

    [Fact]
    public void Standard_item_quality_never_goes_below_zero()
    {
        var item = new ItemBuilder().Standard().WithQuality(0).WithSellIn(5).Build();
        var inn = new GildedRoseInn(new Inventory(new[] { item }));

        inn.UpdateInventory();

        item.Quality.Should().Be(0);
    }
}
