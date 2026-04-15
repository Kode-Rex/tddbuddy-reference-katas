import pytest

from video_club_rental import Age

from .user_builder import UserBuilder
from .video_club_builder import VideoClubBuilder


def test_user_aged_eighteen_registers_successfully():
    club, _, _ = VideoClubBuilder().build()
    user = club.register("Eighteen", "eighteen@example.com", Age(18))
    assert user.age.is_adult
    assert user in club.users


def test_user_aged_seventeen_is_rejected_as_too_young():
    club, _, _ = VideoClubBuilder().build()
    with pytest.raises(ValueError):
        club.register("Seventeen", "seventeen@example.com", Age(17))


def test_registration_dispatches_a_welcome_email():
    club, notifier, _ = VideoClubBuilder().build()
    user = club.register("Alex", "alex@example.com", Age(30))
    notes = notifier.notifications_for(user)
    assert len(notes) == 1
    assert "Welcome" in notes[0].message


def test_admin_creates_another_user_successfully():
    admin = UserBuilder().named("Boss").with_email("boss@example.com").as_admin().build()
    club, _, _ = VideoClubBuilder().with_user(admin).build()
    created = club.create_user(admin, "New Hire", "new@example.com", Age(22))
    assert created in club.users


def test_non_admin_attempting_to_create_a_user_is_rejected():
    regular = UserBuilder().build()
    club, _, _ = VideoClubBuilder().with_user(regular).build()
    with pytest.raises(PermissionError):
        club.create_user(regular, "New Hire", "new@example.com", Age(22))
