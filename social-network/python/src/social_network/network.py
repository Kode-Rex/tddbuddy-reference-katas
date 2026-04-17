from __future__ import annotations

from social_network.clock import Clock
from social_network.post import Post
from social_network.user import User


class Network:
    def __init__(self, clock: Clock) -> None:
        self._clock = clock
        self._users: dict[str, User] = {}
        self._posts: list[Post] = []

    @property
    def users(self) -> dict[str, User]:
        return dict(self._users)

    def post(self, user_name: str, content: str) -> None:
        self._ensure_registered(user_name)
        self._posts.append(Post(user_name, content, self._clock.now()))

    def follow(self, follower_name: str, followee_name: str) -> None:
        self._ensure_registered(follower_name)
        self._ensure_registered(followee_name)
        self._users[follower_name].follow(followee_name)

    def timeline(self, user_name: str) -> list[Post]:
        return sorted(
            [p for p in self._posts if p.author == user_name],
            key=lambda p: p.timestamp,
            reverse=True,
        )

    def wall(self, user_name: str) -> list[Post]:
        user = self._users.get(user_name)
        if user is None:
            return []

        visible = {user_name} | set(user.following)
        return sorted(
            [p for p in self._posts if p.author in visible],
            key=lambda p: p.timestamp,
            reverse=True,
        )

    def _ensure_registered(self, user_name: str) -> None:
        if user_name not in self._users:
            self._users[user_name] = User(user_name)
