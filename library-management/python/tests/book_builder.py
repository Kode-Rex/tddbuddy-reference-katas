from __future__ import annotations

from library_management import Book, Isbn


class BookBuilder:
    def __init__(self) -> None:
        self._title = "The Pragmatic Programmer"
        self._author = "Andrew Hunt"
        self._isbn = Isbn("978-0201616224")
        self._copies = 1

    def titled(self, title: str) -> "BookBuilder":
        self._title = title
        return self

    def by(self, author: str) -> "BookBuilder":
        self._author = author
        return self

    def with_isbn(self, isbn: str) -> "BookBuilder":
        self._isbn = Isbn(isbn)
        return self

    def with_copies(self, copies: int) -> "BookBuilder":
        self._copies = copies
        return self

    def build(self) -> Book:
        return Book(self._title, self._author, self._isbn)

    @property
    def copy_count(self) -> int:
        return self._copies
