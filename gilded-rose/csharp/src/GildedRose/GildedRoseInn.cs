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
            var degrade = item.SellIn <= 0 ? 2 : 1;
            item.Quality = Math.Max(0, item.Quality - degrade);
            item.SellIn -= 1;
        }
    }
}
