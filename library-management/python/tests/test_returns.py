from datetime import date

from library_management import CopyStatus, Isbn, LOAN_PERIOD_DAYS, Money

from .book_builder import BookBuilder
from .library_builder import LibraryBuilder
from .member_builder import MemberBuilder

DAY_0 = date(2026, 1, 1)
ISBN = "978-0134757599"


def _open_library_with_active_loan():
    member = MemberBuilder().build()
    library, notifier, clock = (
        LibraryBuilder().opened_on(DAY_0).with_member(member)
        .with_book(BookBuilder().with_isbn(ISBN).with_copies(1)).build()
    )
    library.check_out(member, Isbn(ISBN))
    return library, notifier, clock, member


def test_returning_a_checked_out_copy_marks_the_copy_as_available():
    library, _, clock, member = _open_library_with_active_loan()
    clock.advance_days(5)
    library.return_copy(member, Isbn(ISBN))
    assert library.books[0].copies[0].status == CopyStatus.AVAILABLE


def test_returning_a_copy_closes_the_loan():
    library, _, clock, member = _open_library_with_active_loan()
    clock.advance_days(5)
    library.return_copy(member, Isbn(ISBN))
    assert library.loans == []


def test_returning_on_time_incurs_no_fine():
    library, _, clock, member = _open_library_with_active_loan()
    clock.advance_days(LOAN_PERIOD_DAYS)
    fine = library.return_copy(member, Isbn(ISBN))
    assert fine == Money.zero()


def test_returning_one_day_late_incurs_a_ten_pence_fine():
    library, _, clock, member = _open_library_with_active_loan()
    clock.advance_days(LOAN_PERIOD_DAYS + 1)
    fine = library.return_copy(member, Isbn(ISBN))
    assert fine == Money("0.10")


def test_returning_ten_days_late_incurs_a_one_pound_fine():
    library, _, clock, member = _open_library_with_active_loan()
    clock.advance_days(LOAN_PERIOD_DAYS + 10)
    fine = library.return_copy(member, Isbn(ISBN))
    assert fine == Money("1.00")
