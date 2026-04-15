from __future__ import annotations

from datetime import date

from .isbn import Isbn
from .member import Member

RESERVATION_EXPIRY_DAYS = 3


class Reservation:
    def __init__(self, member: Member, isbn: Isbn, reserved_on: date) -> None:
        self.member = member
        self.isbn = isbn
        self.reserved_on = reserved_on
        self.notified_on: date | None = None

    @property
    def is_notified(self) -> bool:
        return self.notified_on is not None

    def has_expired_at(self, today: date) -> bool:
        if self.notified_on is None:
            return False
        return (today - self.notified_on).days > RESERVATION_EXPIRY_DAYS

    def mark_notified(self, today: date) -> None:
        self.notified_on = today
