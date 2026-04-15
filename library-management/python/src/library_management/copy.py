from __future__ import annotations

from .copy_status import CopyStatus
from .isbn import Isbn


class Copy:
    def __init__(self, id: int, isbn: Isbn) -> None:
        self.id = id
        self.isbn = isbn
        self.status = CopyStatus.AVAILABLE

    def mark_checked_out(self) -> None:
        self.status = CopyStatus.CHECKED_OUT

    def mark_available(self) -> None:
        self.status = CopyStatus.AVAILABLE

    def mark_reserved(self) -> None:
        self.status = CopyStatus.RESERVED
