namespace LibraryManagement.Tests;

public class BookBuilder
{
    private string _title = "The Pragmatic Programmer";
    private string _author = "Andrew Hunt";
    private Isbn _isbn = new("978-0201616224");
    private int _copies = 1;

    public BookBuilder Titled(string title) { _title = title; return this; }
    public BookBuilder By(string author) { _author = author; return this; }
    public BookBuilder WithIsbn(string isbn) { _isbn = new Isbn(isbn); return this; }
    public BookBuilder WithCopies(int copies) { _copies = copies; return this; }

    public Book Build()
    {
        return new Book(_title, _author, _isbn);
    }

    internal int CopyCount => _copies;
}
