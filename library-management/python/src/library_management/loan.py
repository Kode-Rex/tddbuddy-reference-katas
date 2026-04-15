from __future__ import annotations

from datetime import date, timedelta

from .copy import Copy
from .member import Member
from .money import Money

LOAN_PERIOD_DAYS = 14
FINE_PER_DAY = Money("0.10")


class Loan:
    def __init__(self, member: Member, copy: Copy, borrowed_on: date) -> None:
        self.member = member
        self.copy = copy
        self.borrowed_on = borrowed_on
        self.due_on = borrowed_on + timedelta(days=LOAN_PERIOD_DAYS)
        self.returned_on: date | None = None

    @property
    def is_closed(self) -> bool:
        return self.returned_on is not None

    def fine_for(self, return_date: date) -> Money:
        if return_date <= self.due_on:
            return Money.zero()
        days_late = (return_date - self.due_on).days
        return FINE_PER_DAY * days_late

    def close(self, returned_on: date) -> None:
        self.returned_on = returned_on
