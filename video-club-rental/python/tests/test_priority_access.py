from video_club_rental import RENTAL_PERIOD_DAYS

from .title_builder import TitleBuilder
from .user_builder import UserBuilder
from .video_club_builder import VideoClubBuilder


def test_user_with_five_priority_points_has_priority_access_to_new_releases():
    user = UserBuilder().with_priority_points(5).build()
    club, _, _ = VideoClubBuilder().with_user(user).build()
    assert club.has_priority_access(user)


def test_user_with_four_priority_points_does_not_have_priority_access():
    user = UserBuilder().with_priority_points(4).build()
    club, _, _ = VideoClubBuilder().with_user(user).build()
    assert not club.has_priority_access(user)


def test_priority_points_cannot_go_below_zero():
    user = UserBuilder().with_priority_points(1).build()
    club, _, clock = (
        VideoClubBuilder().with_user(user)
        .with_title(TitleBuilder().named("Jaws").build()).build()
    )
    club.rent(user, "Jaws")
    clock.advance_days(RENTAL_PERIOD_DAYS + 1)
    club.return_title(user, "Jaws")
    assert user.priority_points == 0
