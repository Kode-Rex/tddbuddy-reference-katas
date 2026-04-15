from __future__ import annotations

from video_club_rental import Age, User


class UserBuilder:
    def __init__(self) -> None:
        self._name = "Alex Member"
        self._email = "alex@example.com"
        self._age = Age(30)
        self._is_admin = False
        self._priority_points = 0

    def named(self, name: str) -> "UserBuilder":
        self._name = name
        return self

    def with_email(self, email: str) -> "UserBuilder":
        self._email = email
        return self

    def aged(self, years: int) -> "UserBuilder":
        self._age = Age(years)
        return self

    def as_admin(self) -> "UserBuilder":
        self._is_admin = True
        return self

    def with_priority_points(self, points: int) -> "UserBuilder":
        self._priority_points = points
        return self

    def build(self) -> User:
        user = User(self._name, self._email, self._age, self._is_admin)
        if self._priority_points > 0:
            user.seed_priority_points(self._priority_points)
        return user
