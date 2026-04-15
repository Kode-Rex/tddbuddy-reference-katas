from __future__ import annotations

from datetime import date

from library_management import Library, Member

from .book_builder import BookBuilder
from .fixed_clock import FixedClock
from .recording_notifier import RecordingNotifier


class LibraryBuilder:
    def __init__(self) -> None:
        self._opened_on: date = date(2026, 1, 1)
        self._members: list[Member] = []
        self._books: list[BookBuilder] = []

    def opened_on(self, d: date) -> "LibraryBuilder":
        self._opened_on = d
        return self

    def with_member(self, member: Member) -> "LibraryBuilder":
        self._members.append(member)
        return self

    def with_book(self, builder: BookBuilder) -> "LibraryBuilder":
        self._books.append(builder)
        return self

    def build(self) -> tuple[Library, RecordingNotifier, FixedClock]:
        clock = FixedClock(self._opened_on)
        notifier = RecordingNotifier()
        library = Library(clock, notifier)
        for m in self._members:
            library.seed_member(m)
        for b in self._books:
            library.seed_book(b.build(), b.copy_count)
        return library, notifier, clock
