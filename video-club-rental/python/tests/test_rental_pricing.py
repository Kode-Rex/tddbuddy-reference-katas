from video_club_rental import Money

from .title_builder import TitleBuilder
from .user_builder import UserBuilder
from .video_club_builder import VideoClubBuilder


def _stocked_club():
    user = UserBuilder().build()
    club, _, _ = (
        VideoClubBuilder()
        .with_user(user)
        .with_title(TitleBuilder().named("The Godfather").build())
        .with_title(TitleBuilder().named("Casablanca").build())
        .with_title(TitleBuilder().named("Jaws").build())
        .build()
    )
    return club, user


def test_first_simultaneous_rental_costs_two_pounds_fifty():
    club, user = _stocked_club()
    assert club.rent(user, "The Godfather") == Money("2.50")


def test_second_simultaneous_rental_costs_two_pounds_twenty_five():
    club, user = _stocked_club()
    club.rent(user, "The Godfather")
    assert club.rent(user, "Casablanca") == Money("2.25")


def test_third_simultaneous_rental_costs_one_pound_seventy_five():
    club, user = _stocked_club()
    club.rent(user, "The Godfather")
    club.rent(user, "Casablanca")
    assert club.rent(user, "Jaws") == Money("1.75")


def test_renting_two_titles_charges_four_pounds_seventy_five_total():
    club, user = _stocked_club()
    first = club.rent(user, "The Godfather")
    second = club.rent(user, "Casablanca")
    assert first + second == Money("4.75")


def test_renting_three_titles_charges_six_pounds_fifty_total():
    club, user = _stocked_club()
    first = club.rent(user, "The Godfather")
    second = club.rent(user, "Casablanca")
    third = club.rent(user, "Jaws")
    assert first + second + third == Money("6.50")
