from __future__ import annotations

from dataclasses import dataclass
from datetime import datetime, timedelta, timezone

from social_network import Network

from .fixed_clock import FixedClock


@dataclass
class _SeededPost:
    user: str
    content: str
    minutes_after_start: int


@dataclass
class _SeededFollow:
    follower: str
    followee: str


class NetworkBuilder:
    def __init__(self) -> None:
        self._start_time = datetime(2026, 1, 15, 9, 0, 0, tzinfo=timezone.utc)
        self._seeded_posts: list[_SeededPost] = []
        self._seeded_follows: list[_SeededFollow] = []

    def starting_at(self, dt: datetime) -> NetworkBuilder:
        self._start_time = dt
        return self

    def with_post(self, user: str, content: str, minutes_after_start: int = 0) -> NetworkBuilder:
        self._seeded_posts.append(_SeededPost(user, content, minutes_after_start))
        return self

    def with_follow(self, follower: str, followee: str) -> NetworkBuilder:
        self._seeded_follows.append(_SeededFollow(follower, followee))
        return self

    def build(self) -> tuple[Network, FixedClock]:
        clock = FixedClock(self._start_time)
        network = Network(clock)

        for seed in self._seeded_posts:
            clock.advance_to(self._start_time + timedelta(minutes=seed.minutes_after_start))
            network.post(seed.user, seed.content)

        for seed in self._seeded_follows:
            network.follow(seed.follower, seed.followee)

        return network, clock
