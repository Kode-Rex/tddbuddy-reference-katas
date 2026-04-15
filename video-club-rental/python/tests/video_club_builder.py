from __future__ import annotations

from datetime import date

from video_club_rental import Title, User, VideoClub

from .fixed_clock import FixedClock
from .recording_notifier import RecordingNotifier


class VideoClubBuilder:
    def __init__(self) -> None:
        self._opened_on: date = date(2026, 1, 1)
        self._users: list[User] = []
        self._titles: list[Title] = []

    def opened_on(self, d: date) -> "VideoClubBuilder":
        self._opened_on = d
        return self

    def with_user(self, user: User) -> "VideoClubBuilder":
        self._users.append(user)
        return self

    def with_title(self, title: Title) -> "VideoClubBuilder":
        self._titles.append(title)
        return self

    def build(self) -> tuple[VideoClub, RecordingNotifier, FixedClock]:
        clock = FixedClock(self._opened_on)
        notifier = RecordingNotifier()
        club = VideoClub(clock, notifier)
        for user in self._users:
            club.seed_user(user)
        for title in self._titles:
            club.add_title(title)
        return club, notifier, clock
