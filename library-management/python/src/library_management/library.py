from __future__ import annotations

from .book import Book
from .clock import Clock
from .exceptions import (
    BookNotInCatalogError,
    NoActiveLoanError,
    NoCopiesAvailableError,
)
from .isbn import Isbn
from .loan import Loan
from .member import Member
from .money import Money
from .notifier import Notifier
from .reservation import Reservation


def _reservation_available(title: str) -> str:
    return f"'{title}' is now available to borrow"


def _reservation_expired(title: str) -> str:
    return f"Your reservation for '{title}' has expired"


class Library:
    def __init__(self, clock: Clock, notifier: Notifier) -> None:
        self._clock = clock
        self._notifier = notifier
        self._books: dict[str, Book] = {}
        self._members: list[Member] = []
        self._loans: list[Loan] = []
        self._reservations: list[Reservation] = []
        self._next_member_id = 1

    @property
    def books(self) -> list[Book]:
        return list(self._books.values())

    @property
    def members(self) -> list[Member]:
        return list(self._members)

    @property
    def loans(self) -> list[Loan]:
        return list(self._loans)

    @property
    def reservations(self) -> list[Reservation]:
        return list(self._reservations)

    def add_book(self, title: str, author: str, isbn: Isbn) -> Book:
        existing = self._books.get(isbn.value)
        if existing is not None:
            existing.add_copy()
            return existing
        book = Book(title, author, isbn)
        book.add_copy()
        self._books[isbn.value] = book
        return book

    def add_copy_of(self, isbn: Isbn) -> Book:
        book = self._require_book(isbn)
        book.add_copy()
        return book

    def remove_copy(self, isbn: Isbn) -> None:
        book = self._require_book(isbn)
        book.remove_one_copy()
        if book.copy_count == 0:
            del self._books[isbn.value]

    def register(self, name: str) -> Member:
        member = Member(self._next_member_id, name)
        self._next_member_id += 1
        self._members.append(member)
        return member

    def seed_member(self, member: Member) -> None:
        self._members.append(member)
        if member.id >= self._next_member_id:
            self._next_member_id = member.id + 1

    def seed_book(self, book: Book, copy_count: int) -> Book:
        self._books[book.isbn.value] = book
        for _ in range(copy_count):
            book.add_copy()
        return book

    def check_out(self, member: Member, isbn: Isbn) -> Loan:
        book = self._require_book(isbn)
        today = self._clock.today()

        reserved_copy = book.find_reserved_copy()
        if reserved_copy is not None:
            head = self._head_reservation_for(isbn)
            if head is None or head.member is not member:
                raise NoCopiesAvailableError(f"No copies of '{isbn}' are available")
            self._reservations.remove(head)
            reserved_copy.mark_checked_out()
            reserved_loan = Loan(member, reserved_copy, today)
            self._loans.append(reserved_loan)
            return reserved_loan

        available = book.find_available_copy()
        if available is None:
            raise NoCopiesAvailableError(f"No copies of '{isbn}' are available")
        available.mark_checked_out()
        loan = Loan(member, available, today)
        self._loans.append(loan)
        return loan

    def return_copy(self, member: Member, isbn: Isbn) -> Money:
        loan = next(
            (
                l for l in self._loans
                if not l.is_closed and l.member is member and l.copy.isbn == isbn
            ),
            None,
        )
        if loan is None:
            raise NoActiveLoanError(f"Member has no active loan of '{isbn}'")

        today = self._clock.today()
        loan.close(today)
        self._loans.remove(loan)
        fine = loan.fine_for(today)

        head = self._head_reservation_for(isbn)
        if head is not None:
            loan.copy.mark_reserved()
            head.mark_notified(today)
            self._notifier.send(head.member, _reservation_available(self._book_title(isbn)))
        else:
            loan.copy.mark_available()

        return fine

    def reserve(self, member: Member, isbn: Isbn) -> None:
        self._require_book(isbn)
        self._reservations.append(Reservation(member, isbn, self._clock.today()))

    def expire_reservations(self) -> None:
        today = self._clock.today()
        expired = [r for r in self._reservations if r.has_expired_at(today)]
        for reservation in expired:
            self._reservations.remove(reservation)
            self._notifier.send(
                reservation.member,
                _reservation_expired(self._book_title(reservation.isbn)),
            )

            book = self._books.get(reservation.isbn.value)
            reserved_copy = book.find_reserved_copy() if book is not None else None
            if reserved_copy is None:
                continue

            nxt = self._head_reservation_for(reservation.isbn)
            if nxt is not None:
                nxt.mark_notified(today)
                self._notifier.send(
                    nxt.member,
                    _reservation_available(self._book_title(reservation.isbn)),
                )
            else:
                reserved_copy.mark_available()

    def reservations_for(self, isbn: Isbn) -> list[Reservation]:
        return [r for r in self._reservations if r.isbn == isbn]

    def _head_reservation_for(self, isbn: Isbn) -> Reservation | None:
        return next((r for r in self._reservations if r.isbn == isbn), None)

    def _require_book(self, isbn: Isbn) -> Book:
        book = self._books.get(isbn.value)
        if book is None:
            raise BookNotInCatalogError(f"Book '{isbn}' is not in the catalog")
        return book

    def _book_title(self, isbn: Isbn) -> str:
        book = self._books.get(isbn.value)
        return book.title if book is not None else isbn.value
