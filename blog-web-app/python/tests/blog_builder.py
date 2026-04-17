from __future__ import annotations

from dataclasses import dataclass
from datetime import datetime, timedelta, timezone

from blog_web_app import Blog, Post

from .fixed_clock import FixedClock


@dataclass
class _SeededPost:
    user: str
    title: str
    body: str
    minutes_after_start: int


@dataclass
class _SeededComment:
    post_index: int
    user: str
    body: str
    minutes_after_start: int


@dataclass
class _SeededTag:
    post_index: int
    tag: str


class BlogBuilder:
    def __init__(self) -> None:
        self._start_time = datetime(2026, 1, 15, 9, 0, 0, tzinfo=timezone.utc)
        self._seeded_posts: list[_SeededPost] = []
        self._seeded_comments: list[_SeededComment] = []
        self._seeded_tags: list[_SeededTag] = []

    def starting_at(self, dt: datetime) -> BlogBuilder:
        self._start_time = dt
        return self

    def with_post(
        self, user: str, title: str, body: str, minutes_after_start: int = 0
    ) -> BlogBuilder:
        self._seeded_posts.append(_SeededPost(user, title, body, minutes_after_start))
        return self

    def with_comment(
        self, post_index: int, user: str, body: str, minutes_after_start: int = 0
    ) -> BlogBuilder:
        self._seeded_comments.append(
            _SeededComment(post_index, user, body, minutes_after_start)
        )
        return self

    def with_tag(self, post_index: int, tag: str) -> BlogBuilder:
        self._seeded_tags.append(_SeededTag(post_index, tag))
        return self

    def build(self) -> tuple[Blog, FixedClock, list[Post]]:
        clock = FixedClock(self._start_time)
        blog = Blog(clock)
        posts: list[Post] = []

        for seed in self._seeded_posts:
            clock.advance_to(
                self._start_time + timedelta(minutes=seed.minutes_after_start)
            )
            posts.append(blog.create_post(seed.user, seed.title, seed.body))

        for seed in self._seeded_comments:
            clock.advance_to(
                self._start_time + timedelta(minutes=seed.minutes_after_start)
            )
            blog.add_comment(seed.user, posts[seed.post_index].id, seed.body)

        for seed in self._seeded_tags:
            blog.add_tag(
                posts[seed.post_index].author,
                posts[seed.post_index].id,
                seed.tag,
            )

        return blog, clock, posts
