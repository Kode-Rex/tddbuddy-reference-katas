from .title_builder import TitleBuilder
from .user_builder import UserBuilder
from .video_club_builder import VideoClubBuilder


def test_donating_a_new_title_creates_a_library_entry_with_one_copy():
    donor = UserBuilder().build()
    club, _, _ = VideoClubBuilder().with_user(donor).build()
    club.donate(donor, "Rushmore")
    rushmore = next(t for t in club.titles if t.name == "Rushmore")
    assert rushmore.total_copies == 1


def test_donating_an_existing_title_increments_its_copy_count():
    donor = UserBuilder().build()
    club, _, _ = (
        VideoClubBuilder().with_user(donor)
        .with_title(TitleBuilder().named("Jaws").with_copies(2).build()).build()
    )
    club.donate(donor, "Jaws")
    jaws = next(t for t in club.titles if t.name == "Jaws")
    assert jaws.total_copies == 3
    assert jaws.available_copies == 3


def test_donating_awards_ten_loyalty_points_to_the_donor():
    donor = UserBuilder().build()
    club, _, _ = VideoClubBuilder().with_user(donor).build()
    club.donate(donor, "Rushmore")
    assert donor.loyalty_points == 10
