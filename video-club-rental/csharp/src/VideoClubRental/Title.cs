namespace VideoClubRental;

public class Title
{
    public Title(string name, int totalCopies)
    {
        if (totalCopies < 0) throw new ArgumentException("totalCopies must be non-negative", nameof(totalCopies));
        Name = name;
        TotalCopies = totalCopies;
        AvailableCopies = totalCopies;
    }

    public string Name { get; }
    public int TotalCopies { get; private set; }
    public int AvailableCopies { get; private set; }

    internal void AddCopy()
    {
        TotalCopies++;
        AvailableCopies++;
    }

    internal void CheckOut()
    {
        if (AvailableCopies <= 0) throw new NoCopiesAvailableException($"No copies of '{Name}' available");
        AvailableCopies--;
    }

    internal void CheckIn() => AvailableCopies++;
}
