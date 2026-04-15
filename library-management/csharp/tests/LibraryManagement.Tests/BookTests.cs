using FluentAssertions;

namespace LibraryManagement.Tests;

public class BookTests
{
    private static readonly DateOnly Day0 = new(2026, 1, 1);

    [Fact]
    public void New_library_has_no_books()
    {
        var (library, _, _) = new LibraryBuilder().OpenedOn(Day0).Build();

        library.Books.Should().BeEmpty();
    }

    [Fact]
    public void Adding_a_book_with_one_copy_makes_the_book_available()
    {
        var (library, _, _) = new LibraryBuilder().OpenedOn(Day0).Build();

        var book = library.AddBook("Refactoring", "Martin Fowler", new Isbn("978-0134757599"));

        library.Books.Should().ContainSingle();
        book.CopyCount.Should().Be(1);
        book.Copies[0].Status.Should().Be(CopyStatus.Available);
    }

    [Fact]
    public void Adding_another_copy_of_an_existing_book_increments_the_copy_count()
    {
        var (library, _, _) = new LibraryBuilder().OpenedOn(Day0).Build();
        var isbn = new Isbn("978-0134757599");
        library.AddBook("Refactoring", "Martin Fowler", isbn);

        library.AddCopyOf(isbn);

        library.Books.Single().CopyCount.Should().Be(2);
    }

    [Fact]
    public void Removing_a_copy_decrements_the_copy_count()
    {
        var (library, _, _) = new LibraryBuilder().OpenedOn(Day0)
            .WithBook(new BookBuilder().WithIsbn("978-0134757599").WithCopies(3)).Build();
        var isbn = new Isbn("978-0134757599");

        library.RemoveCopy(isbn);

        library.Books.Single().CopyCount.Should().Be(2);
    }

    [Fact]
    public void Removing_the_last_copy_removes_the_book_from_the_catalog()
    {
        var (library, _, _) = new LibraryBuilder().OpenedOn(Day0)
            .WithBook(new BookBuilder().WithIsbn("978-0134757599").WithCopies(1)).Build();
        var isbn = new Isbn("978-0134757599");

        library.RemoveCopy(isbn);

        library.Books.Should().BeEmpty();
    }
}
