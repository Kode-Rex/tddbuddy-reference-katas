namespace LibraryManagement.Tests;

public class LibraryBuilder
{
    private DateOnly _openedOn = new(2026, 1, 1);
    private readonly List<Member> _members = new();
    private readonly List<(BookBuilder Builder, Book Built)> _books = new();

    public LibraryBuilder OpenedOn(DateOnly date) { _openedOn = date; return this; }

    public LibraryBuilder WithMember(Member member) { _members.Add(member); return this; }

    public LibraryBuilder WithBook(BookBuilder builder)
    {
        _books.Add((builder, builder.Build()));
        return this;
    }

    public (Library Library, RecordingNotifier Notifier, FixedClock Clock) Build()
    {
        var clock = new FixedClock(_openedOn);
        var notifier = new RecordingNotifier();
        var library = new Library(clock, notifier);
        foreach (var member in _members) library.SeedMember(member);
        foreach (var (builder, book) in _books) library.SeedBook(book, builder.CopyCount);
        return (library, notifier, clock);
    }
}
