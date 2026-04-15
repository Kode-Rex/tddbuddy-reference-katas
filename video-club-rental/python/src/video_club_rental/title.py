from __future__ import annotations

from .exceptions import NoCopiesAvailableError


class Title:
    def __init__(self, name: str, total_copies: int) -> None:
        if total_copies < 0:
            raise ValueError("total_copies must be non-negative")
        self.name = name
        self._total_copies = total_copies
        self._available_copies = total_copies

    @property
    def total_copies(self) -> int:
        return self._total_copies

    @property
    def available_copies(self) -> int:
        return self._available_copies

    def add_copy(self) -> None:
        self._total_copies += 1
        self._available_copies += 1

    def check_out(self) -> None:
        if self._available_copies <= 0:
            raise NoCopiesAvailableError(f"No copies of '{self.name}' available")
        self._available_copies -= 1

    def check_in(self) -> None:
        self._available_copies += 1
