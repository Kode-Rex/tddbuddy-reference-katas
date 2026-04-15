namespace LibraryManagement;

public readonly record struct Isbn(string Value)
{
    public override string ToString() => Value;
}
