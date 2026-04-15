namespace GildedRose;

public class GildedRoseInn
{
    public Inventory Inventory { get; }

    public GildedRoseInn(Inventory inventory)
    {
        Inventory = inventory;
    }

    public void UpdateInventory()
    {
        foreach (var item in Inventory.Items)
        {
            if (item.Category == Category.Legendary)
            {
                continue;
            }
            if (item.Category == Category.Aged)
            {
                var gain = item.SellIn <= 0 ? 2 : 1;
                item.Quality = Math.Min(50, item.Quality + gain);
            }
            else if (item.Category == Category.BackstagePass)
            {
                if (item.SellIn <= 0)
                {
                    item.Quality = 0;
                }
                else
                {
                    var gain = item.SellIn <= 5 ? 3
                        : item.SellIn <= 10 ? 2
                        : 1;
                    item.Quality = Math.Min(50, item.Quality + gain);
                }
            }
            else if (item.Category == Category.Conjured)
            {
                var degrade = item.SellIn <= 0 ? 4 : 2;
                item.Quality = Math.Max(0, item.Quality - degrade);
            }
            else
            {
                var degrade = item.SellIn <= 0 ? 2 : 1;
                item.Quality = Math.Max(0, item.Quality - degrade);
            }
            item.SellIn -= 1;
        }
    }
}
