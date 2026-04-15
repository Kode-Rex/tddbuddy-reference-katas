from __future__ import annotations

from .age import Age, AGE_ADULT_MINIMUM
from .clock import Clock
from .money import Money
from .notifier import Notifier
from . import pricing_policy
from .rental import Rental
from .title import Title
from .user import User

PRIORITY_ACCESS_THRESHOLD = 5
ON_TIME_RETURN_AWARD = 2
LATE_RETURN_PENALTY = 2
DONATION_LOYALTY_AWARD = 10

_WELCOME_MESSAGE = "Welcome to the video club"


def _late_alert(title: str) -> str:
    return f"Your rental of '{title}' is overdue"


def _wishlist_available(title: str) -> str:
    return f"'{title}' is now available"


class VideoClub:
    def __init__(self, clock: Clock, notifier: Notifier) -> None:
        self._clock = clock
        self._notifier = notifier
        self._users: dict[str, User] = {}
        self._titles: dict[str, Title] = {}
        self._rentals: list[Rental] = []

    @property
    def users(self) -> list[User]:
        return list(self._users.values())

    @property
    def titles(self) -> list[Title]:
        return list(self._titles.values())

    def register(self, name: str, email: str, age: Age) -> User:
        if not age.is_adult:
            raise ValueError(f"User must be at least {AGE_ADULT_MINIMUM} to register")
        user = User(name, email, age)
        self._users[email.lower()] = user
        self._notifier.send(user, _WELCOME_MESSAGE)
        return user

    def create_user(self, admin: User, name: str, email: str, age: Age) -> User:
        if not admin.is_admin:
            raise PermissionError("Only admin users may create other users")
        return self.register(name, email, age)

    def seed_user(self, user: User) -> None:
        self._users[user.email.lower()] = user

    def add_title(self, title: Title) -> Title:
        self._titles[title.name.lower()] = title
        return title

    def rent(self, user: User, title_name: str) -> Money:
        if user.has_overdue:
            raise RuntimeError("User has an overdue rental and cannot rent")
        title = self._require_title(title_name)
        existing = len(self._active_rentals_for(user))
        cost = pricing_policy.calculate(new_rental_count=1, existing_rental_count=existing)
        title.check_out()
        self._rentals.append(Rental(user, title, self._clock.today()))
        return cost

    def return_title(self, user: User, title_name: str) -> None:
        rental = next(
            (
                r for r in self._active_rentals_for(user)
                if r.title.name.lower() == title_name.lower()
            ),
            None,
        )
        if rental is None:
            raise RuntimeError(f"User has no active rental of '{title_name}'")

        today = self._clock.today()
        self._rentals.remove(rental)
        rental.title.check_in()

        if rental.is_late_at(today):
            user.deduct_priority_points(LATE_RETURN_PENALTY)
            self._notifier.send(user, _late_alert(rental.title.name))
        else:
            user.award_priority_points(ON_TIME_RETURN_AWARD)

        if not any(r.is_late_at(today) for r in self._active_rentals_for(user)):
            user.clear_overdue()

    def mark_overdue_rentals(self) -> None:
        today = self._clock.today()
        for rental in self._rentals:
            if rental.is_late_at(today):
                rental.user.mark_overdue()

    def has_priority_access(self, user: User) -> bool:
        return user.priority_points >= PRIORITY_ACCESS_THRESHOLD

    def add_to_wishlist(self, user: User, title_name: str) -> None:
        user.add_wish(title_name)

    def donate(self, donor: User, title_name: str) -> None:
        existing = self._titles.get(title_name.lower())
        if existing is not None:
            existing.add_copy()
        else:
            self.add_title(Title(title_name, total_copies=1))

        donor.award_loyalty_points(DONATION_LOYALTY_AWARD)

        for user in self._users.values():
            if user.wishes(title_name):
                self._notifier.send(user, _wishlist_available(title_name))

    def _require_title(self, title_name: str) -> Title:
        title = self._titles.get(title_name.lower())
        if title is None:
            raise RuntimeError(f"Title '{title_name}' is not in the catalog")
        return title

    def _active_rentals_for(self, user: User) -> list[Rental]:
        return [r for r in self._rentals if r.user is user]
