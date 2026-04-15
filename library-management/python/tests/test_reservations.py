from datetime import date

from library_management import CopyStatus, Isbn, RESERVATION_EXPIRY_DAYS

from .book_builder import BookBuilder
from .library_builder import LibraryBuilder
from .member_builder import MemberBuilder

DAY_0 = date(2026, 1, 1)
ISBN = "978-0134757599"


def test_reserving_an_unavailable_book_adds_the_member_to_the_queue():
    borrower = MemberBuilder().named("Borrower").build()
    reserver = MemberBuilder().named("Reserver").build()
    library, _, _ = (
        LibraryBuilder().opened_on(DAY_0)
        .with_member(borrower).with_member(reserver)
        .with_book(BookBuilder().with_isbn(ISBN).with_copies(1)).build()
    )
    library.check_out(borrower, Isbn(ISBN))

    library.reserve(reserver, Isbn(ISBN))

    queue = library.reservations_for(Isbn(ISBN))
    assert len(queue) == 1
    assert queue[0].member is reserver


def test_returning_a_copy_with_a_non_empty_queue_marks_the_copy_as_reserved():
    borrower = MemberBuilder().named("Borrower").build()
    reserver = MemberBuilder().named("Reserver").build()
    library, _, clock = (
        LibraryBuilder().opened_on(DAY_0)
        .with_member(borrower).with_member(reserver)
        .with_book(BookBuilder().with_isbn(ISBN).with_copies(1)).build()
    )
    library.check_out(borrower, Isbn(ISBN))
    library.reserve(reserver, Isbn(ISBN))
    clock.advance_days(2)

    library.return_copy(borrower, Isbn(ISBN))

    assert library.books[0].copies[0].status == CopyStatus.RESERVED


def test_returning_a_copy_with_a_non_empty_queue_notifies_the_head_of_the_queue():
    borrower = MemberBuilder().named("Borrower").build()
    reserver = MemberBuilder().named("Reserver").build()
    library, notifier, clock = (
        LibraryBuilder().opened_on(DAY_0)
        .with_member(borrower).with_member(reserver)
        .with_book(BookBuilder().titled("Refactoring").with_isbn(ISBN).with_copies(1)).build()
    )
    library.check_out(borrower, Isbn(ISBN))
    library.reserve(reserver, Isbn(ISBN))
    clock.advance_days(2)

    library.return_copy(borrower, Isbn(ISBN))

    notes = notifier.availability_notifications_for(reserver)
    assert len(notes) == 1
    assert "Refactoring" in notes[0].message


def test_reservations_older_than_three_days_expire_and_the_next_reserver_is_notified():
    borrower = MemberBuilder().named("Borrower").build()
    first = MemberBuilder().named("First").build()
    second = MemberBuilder().named("Second").build()
    library, notifier, clock = (
        LibraryBuilder().opened_on(DAY_0)
        .with_member(borrower).with_member(first).with_member(second)
        .with_book(BookBuilder().titled("Refactoring").with_isbn(ISBN).with_copies(1)).build()
    )
    library.check_out(borrower, Isbn(ISBN))
    library.reserve(first, Isbn(ISBN))
    library.reserve(second, Isbn(ISBN))
    library.return_copy(borrower, Isbn(ISBN))  # first is notified

    clock.advance_days(RESERVATION_EXPIRY_DAYS + 1)
    library.expire_reservations()

    assert len(notifier.expiration_notifications_for(first)) == 1
    assert len(notifier.availability_notifications_for(second)) == 1


def test_checking_out_a_reserved_copy_satisfies_the_reservation_and_clears_it():
    borrower = MemberBuilder().named("Borrower").build()
    reserver = MemberBuilder().named("Reserver").build()
    library, _, clock = (
        LibraryBuilder().opened_on(DAY_0)
        .with_member(borrower).with_member(reserver)
        .with_book(BookBuilder().with_isbn(ISBN).with_copies(1)).build()
    )
    library.check_out(borrower, Isbn(ISBN))
    library.reserve(reserver, Isbn(ISBN))
    library.return_copy(borrower, Isbn(ISBN))
    clock.advance_days(1)

    loan = library.check_out(reserver, Isbn(ISBN))

    assert loan.copy.status == CopyStatus.CHECKED_OUT
    assert library.reservations_for(Isbn(ISBN)) == []
