namespace HundredDoors;

public static class HundredDoors
{
    public static IReadOnlyList<int> OpenDoors(int numDoors)
    {
        var isOpen = new bool[numDoors + 1];
        for (var pass = 1; pass <= numDoors; pass++)
        {
            for (var door = pass; door <= numDoors; door += pass)
            {
                isOpen[door] = !isOpen[door];
            }
        }

        var open = new List<int>();
        for (var door = 1; door <= numDoors; door++)
        {
            if (isOpen[door]) open.Add(door);
        }
        return open;
    }
}
