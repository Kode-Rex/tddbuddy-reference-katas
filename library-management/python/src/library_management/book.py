from __future__ import annotations

from .copy import Copy
from .copy_status import CopyStatus
from .isbn import Isbn


class Book:
    def __init__(self, title: str, author: str, isbn: Isbn) -> None:
        self.title = title
        self.author = author
        self.isbn = isbn
        self._copies: list[Copy] = []
        self._next_copy_id = 1

    @property
    def copies(self) -> list[Copy]:
        return list(self._copies)

    @property
    def copy_count(self) -> int:
        return len(self._copies)

    def add_copy(self) -> Copy:
        copy = Copy(self._next_copy_id, self.isbn)
        self._next_copy_id += 1
        self._copies.append(copy)
        return copy

    def remove_one_copy(self) -> None:
        available = next((c for c in self._copies if c.status == CopyStatus.AVAILABLE), None)
        target = available if available is not None else (self._copies[0] if self._copies else None)
        if target is not None:
            self._copies.remove(target)

    def find_available_copy(self) -> Copy | None:
        return next((c for c in self._copies if c.status == CopyStatus.AVAILABLE), None)

    def find_reserved_copy(self) -> Copy | None:
        return next((c for c in self._copies if c.status == CopyStatus.RESERVED), None)
