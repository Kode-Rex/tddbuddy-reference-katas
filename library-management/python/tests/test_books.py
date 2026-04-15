from datetime import date

from library_management import CopyStatus, Isbn

from .book_builder import BookBuilder
from .library_builder import LibraryBuilder

DAY_0 = date(2026, 1, 1)


def test_new_library_has_no_books():
    library, _, _ = LibraryBuilder().opened_on(DAY_0).build()
    assert library.books == []


def test_adding_a_book_with_one_copy_makes_the_book_available():
    library, _, _ = LibraryBuilder().opened_on(DAY_0).build()
    book = library.add_book("Refactoring", "Martin Fowler", Isbn("978-0134757599"))
    assert len(library.books) == 1
    assert book.copy_count == 1
    assert book.copies[0].status == CopyStatus.AVAILABLE


def test_adding_another_copy_of_an_existing_book_increments_the_copy_count():
    library, _, _ = LibraryBuilder().opened_on(DAY_0).build()
    isbn = Isbn("978-0134757599")
    library.add_book("Refactoring", "Martin Fowler", isbn)
    library.add_copy_of(isbn)
    assert library.books[0].copy_count == 2


def test_removing_a_copy_decrements_the_copy_count():
    library, _, _ = (
        LibraryBuilder().opened_on(DAY_0)
        .with_book(BookBuilder().with_isbn("978-0134757599").with_copies(3))
        .build()
    )
    library.remove_copy(Isbn("978-0134757599"))
    assert library.books[0].copy_count == 2


def test_removing_the_last_copy_removes_the_book_from_the_catalog():
    library, _, _ = (
        LibraryBuilder().opened_on(DAY_0)
        .with_book(BookBuilder().with_isbn("978-0134757599").with_copies(1))
        .build()
    )
    library.remove_copy(Isbn("978-0134757599"))
    assert library.books == []
