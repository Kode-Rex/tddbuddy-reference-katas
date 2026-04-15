namespace LibraryManagement;

public class Copy
{
    public Copy(int id, Isbn isbn)
    {
        Id = id;
        Isbn = isbn;
        Status = CopyStatus.Available;
    }

    public int Id { get; }
    public Isbn Isbn { get; }
    public CopyStatus Status { get; private set; }

    internal void MarkCheckedOut() => Status = CopyStatus.CheckedOut;
    internal void MarkAvailable() => Status = CopyStatus.Available;
    internal void MarkReserved() => Status = CopyStatus.Reserved;
}
