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
}
