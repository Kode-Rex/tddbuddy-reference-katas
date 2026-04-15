from .user_builder import UserBuilder
from .video_club_builder import VideoClubBuilder


def test_user_can_add_a_title_to_their_wishlist():
    user = UserBuilder().build()
    club, _, _ = VideoClubBuilder().with_user(user).build()
    club.add_to_wishlist(user, "Rushmore")
    assert user.wishes("Rushmore")


def test_wishlist_matching_is_case_insensitive():
    user = UserBuilder().build()
    donor = UserBuilder().named("Donor").with_email("d@example.com").build()
    club, notifier, _ = VideoClubBuilder().with_user(user).with_user(donor).build()
    club.add_to_wishlist(user, "rushmore")
    club.donate(donor, "RUSHMORE")
    assert len(notifier.notifications_for(user)) == 1


def test_donating_a_wishlisted_title_notifies_the_wishlisting_user():
    user = UserBuilder().build()
    donor = UserBuilder().named("Donor").with_email("d@example.com").build()
    club, notifier, _ = VideoClubBuilder().with_user(user).with_user(donor).build()
    club.add_to_wishlist(user, "Rushmore")
    club.donate(donor, "Rushmore")
    notes = notifier.notifications_for(user)
    assert len(notes) == 1
    assert "available" in notes[0].message
