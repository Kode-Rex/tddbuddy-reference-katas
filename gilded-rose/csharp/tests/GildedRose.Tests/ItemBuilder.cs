namespace GildedRose.Tests;

public class ItemBuilder
{
    private string _name = "Elixir of the Mongoose";
    private Category _category = Category.Standard;
    private int _quality = 10;
    private int _sellIn = 5;

    public ItemBuilder Standard() { _category = Category.Standard; return this; }
    public ItemBuilder Aged() { _category = Category.Aged; return this; }
    public ItemBuilder Legendary() { _category = Category.Legendary; return this; }
    public ItemBuilder BackstagePass() { _category = Category.BackstagePass; return this; }
    public ItemBuilder Conjured() { _category = Category.Conjured; return this; }

    public ItemBuilder Named(string name) { _name = name; return this; }
    public ItemBuilder WithQuality(int quality) { _quality = quality; return this; }
    public ItemBuilder WithSellIn(int sellIn) { _sellIn = sellIn; return this; }

    public Item Build() => new(_name, _category, _quality, _sellIn);
}
