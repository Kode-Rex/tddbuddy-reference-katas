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
}
