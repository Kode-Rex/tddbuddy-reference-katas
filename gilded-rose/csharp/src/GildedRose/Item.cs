namespace GildedRose;

public class Item
{
    public string Name { get; }
    public Category Category { get; }
    public int Quality { get; internal set; }
    public int SellIn { get; internal set; }

    public Item(string name, Category category, int quality, int sellIn)
    {
        Name = name;
        Category = category;
        Quality = quality;
        SellIn = sellIn;
    }
}
