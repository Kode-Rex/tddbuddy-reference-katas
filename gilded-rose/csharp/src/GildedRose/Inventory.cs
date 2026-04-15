namespace GildedRose;

public class Inventory
{
    private readonly List<Item> _items;

    public Inventory(IEnumerable<Item> items)
    {
        _items = items.ToList();
    }

    public IReadOnlyList<Item> Items => _items;
}
