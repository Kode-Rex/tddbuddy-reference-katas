using FluentAssertions;
using Xunit;

namespace GildedRose.Tests;

public class MultiItemWorldTests
{
    [Fact]
    public void Mixed_inventory_each_item_follows_its_own_category_rules_on_the_same_day()
    {
        var standard = new ItemBuilder().Standard().Named("Elixir of the Mongoose").WithQuality(10).WithSellIn(5).Build();
        var aged = new ItemBuilder().Aged().Named("Aged Brie").WithQuality(10).WithSellIn(5).Build();
        var legendary = new ItemBuilder().Legendary().Named("Sulfuras, Hand of Ragnaros").WithQuality(80).WithSellIn(5).Build();
        var pass = new ItemBuilder().BackstagePass().Named("Backstage passes to a TAFKAL80ETC concert").WithQuality(10).WithSellIn(7).Build();
        var conjured = new ItemBuilder().Conjured().Named("Conjured Mana Cake").WithQuality(10).WithSellIn(5).Build();

        var inn = new GildedRoseInn(new Inventory(new[] { standard, aged, legendary, pass, conjured }));

        inn.UpdateInventory();

        standard.Quality.Should().Be(9);
        aged.Quality.Should().Be(11);
        legendary.Quality.Should().Be(80);
        pass.Quality.Should().Be(12);
        conjured.Quality.Should().Be(8);
    }
}
