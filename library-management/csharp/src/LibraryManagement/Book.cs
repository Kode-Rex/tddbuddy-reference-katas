namespace LibraryManagement;

public class Book
{
    private readonly List<Copy> _copies = new();
    private int _nextCopyId = 1;

    public Book(string title, string author, Isbn isbn)
    {
        Title = title;
        Author = author;
        Isbn = isbn;
    }

    public string Title { get; }
    public string Author { get; }
    public Isbn Isbn { get; }

    public IReadOnlyList<Copy> Copies => _copies;
    public int CopyCount => _copies.Count;

    internal Copy AddCopy()
    {
        var copy = new Copy(_nextCopyId++, Isbn);
        _copies.Add(copy);
        return copy;
    }

    internal bool RemoveOneCopy()
    {
        var copy = _copies.FirstOrDefault(c => c.Status == CopyStatus.Available);
        if (copy is null)
        {
            if (_copies.Count == 0) return false;
            copy = _copies[0];
        }
        _copies.Remove(copy);
        return true;
    }

    internal Copy? FindAvailableCopy() => _copies.FirstOrDefault(c => c.Status == CopyStatus.Available);

    internal Copy? FindReservedCopy() => _copies.FirstOrDefault(c => c.Status == CopyStatus.Reserved);
}
