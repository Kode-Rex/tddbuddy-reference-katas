from datetime import date

import pytest

from video_club_rental import Money, RENTAL_PERIOD_DAYS

from .title_builder import TitleBuilder
from .user_builder import UserBuilder
from .video_club_builder import VideoClubBuilder

DAY_0 = date(2026, 1, 1)


def test_on_time_return_awards_two_priority_points():
    user = UserBuilder().build()
    club, _, clock = (
        VideoClubBuilder().opened_on(DAY_0).with_user(user)
        .with_title(TitleBuilder().named("Jaws").build()).build()
    )
    club.rent(user, "Jaws")
    clock.advance_days(RENTAL_PERIOD_DAYS)
    club.return_title(user, "Jaws")
    assert user.priority_points == 2


def test_late_return_deducts_two_priority_points():
    user = UserBuilder().with_priority_points(4).build()
    club, _, clock = (
        VideoClubBuilder().opened_on(DAY_0).with_user(user)
        .with_title(TitleBuilder().named("Jaws").build()).build()
    )
    club.rent(user, "Jaws")
    clock.advance_days(RENTAL_PERIOD_DAYS + 1)
    club.return_title(user, "Jaws")
    assert user.priority_points == 2


def test_late_return_dispatches_a_late_alert():
    user = UserBuilder().build()
    club, notifier, clock = (
        VideoClubBuilder().opened_on(DAY_0).with_user(user)
        .with_title(TitleBuilder().named("Jaws").build()).build()
    )
    club.rent(user, "Jaws")
    clock.advance_days(RENTAL_PERIOD_DAYS + 3)
    club.return_title(user, "Jaws")
    notes = notifier.notifications_for(user)
    assert len(notes) == 1
    assert "overdue" in notes[0].message


def test_user_with_an_overdue_rental_cannot_rent_another_title():
    user = UserBuilder().build()
    club, _, clock = (
        VideoClubBuilder().opened_on(DAY_0).with_user(user)
        .with_title(TitleBuilder().named("Jaws").build())
        .with_title(TitleBuilder().named("Casablanca").build()).build()
    )
    club.rent(user, "Jaws")
    clock.advance_days(RENTAL_PERIOD_DAYS + 1)
    club.mark_overdue_rentals()
    with pytest.raises(RuntimeError):
        club.rent(user, "Casablanca")


def test_returning_the_overdue_title_unblocks_renting():
    user = UserBuilder().build()
    club, _, clock = (
        VideoClubBuilder().opened_on(DAY_0).with_user(user)
        .with_title(TitleBuilder().named("Jaws").build())
        .with_title(TitleBuilder().named("Casablanca").build()).build()
    )
    club.rent(user, "Jaws")
    clock.advance_days(RENTAL_PERIOD_DAYS + 1)
    club.mark_overdue_rentals()
    club.return_title(user, "Jaws")
    assert club.rent(user, "Casablanca") == Money("2.50")
