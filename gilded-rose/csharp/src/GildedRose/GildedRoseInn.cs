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
            item.Quality -= 1;
        }
    }
}
