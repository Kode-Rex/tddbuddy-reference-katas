from __future__ import annotations

from .age import Age


class User:
    def __init__(
        self,
        name: str,
        email: str,
        age: Age,
        is_admin: bool = False,
    ) -> None:
        self.name = name
        self.email = email
        self.age = age
        self.is_admin = is_admin
        self._priority_points = 0
        self._loyalty_points = 0
        self._has_overdue = False
        self._wishlist: set[str] = set()

    @property
    def priority_points(self) -> int:
        return self._priority_points

    @property
    def loyalty_points(self) -> int:
        return self._loyalty_points

    @property
    def has_overdue(self) -> bool:
        return self._has_overdue

    @property
    def wishlist(self) -> frozenset[str]:
        return frozenset(self._wishlist)

    def award_priority_points(self, points: int) -> None:
        self._priority_points += points

    def deduct_priority_points(self, points: int) -> None:
        self._priority_points = max(0, self._priority_points - points)

    def award_loyalty_points(self, points: int) -> None:
        self._loyalty_points += points

    def mark_overdue(self) -> None:
        self._has_overdue = True

    def clear_overdue(self) -> None:
        self._has_overdue = False

    def add_wish(self, title_name: str) -> None:
        self._wishlist.add(title_name.lower())

    def wishes(self, title_name: str) -> bool:
        return title_name.lower() in self._wishlist

    # Test-only: seed priority points before scenario begins.
    def seed_priority_points(self, points: int) -> None:
        self._priority_points = points
