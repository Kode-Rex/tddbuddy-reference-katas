from __future__ import annotations

from datetime import date, timedelta

from .title import Title
from .user import User

RENTAL_PERIOD_DAYS = 15


class Rental:
    def __init__(self, user: User, title: Title, rented_on: date) -> None:
        self.user = user
        self.title = title
        self.rented_on = rented_on
        self.due_on = rented_on + timedelta(days=RENTAL_PERIOD_DAYS)

    def is_late_at(self, return_date: date) -> bool:
        return return_date > self.due_on
