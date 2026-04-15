from datetime import date, timedelta

import pytest

from library_management import CopyStatus, Isbn, LOAN_PERIOD_DAYS

from .book_builder import BookBuilder
from .library_builder import LibraryBuilder
from .member_builder import MemberBuilder

DAY_0 = date(2026, 1, 1)
ISBN = "978-0134757599"


def test_checking_out_an_available_copy_marks_the_copy_as_checked_out():
    member = MemberBuilder().build()
    library, _, _ = (
        LibraryBuilder().opened_on(DAY_0).with_member(member)
        .with_book(BookBuilder().with_isbn(ISBN).with_copies(1)).build()
    )
    loan = library.check_out(member, Isbn(ISBN))
    assert loan.copy.status == CopyStatus.CHECKED_OUT


def test_checking_out_creates_a_loan_with_a_due_date_fourteen_days_from_today():
    member = MemberBuilder().build()
    library, _, _ = (
        LibraryBuilder().opened_on(DAY_0).with_member(member)
        .with_book(BookBuilder().with_isbn(ISBN).with_copies(1)).build()
    )
    loan = library.check_out(member, Isbn(ISBN))
    assert loan.borrowed_on == DAY_0
    assert loan.due_on == DAY_0 + timedelta(days=LOAN_PERIOD_DAYS)


def test_checking_out_when_no_copy_is_available_is_rejected():
    member = MemberBuilder().build()
    other = MemberBuilder().named("Other").build()
    library, _, _ = (
        LibraryBuilder().opened_on(DAY_0)
        .with_member(member).with_member(other)
        .with_book(BookBuilder().with_isbn(ISBN).with_copies(1)).build()
    )
    library.check_out(other, Isbn(ISBN))
    with pytest.raises(RuntimeError):
        library.check_out(member, Isbn(ISBN))
