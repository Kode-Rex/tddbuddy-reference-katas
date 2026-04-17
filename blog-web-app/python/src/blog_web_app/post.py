from __future__ import annotations

from datetime import datetime

from blog_web_app.comment import Comment


class Post:
    def __init__(self, id: int, title: str, body: str, author: str, timestamp: datetime) -> None:
        self.id = id
        self._title = title
        self._body = body
        self.author = author
        self.timestamp = timestamp
        self._comments: list[Comment] = []
        self._tags: set[str] = set()

    @property
    def title(self) -> str:
        return self._title

    @property
    def body(self) -> str:
        return self._body

    @property
    def comments(self) -> list[Comment]:
        return list(self._comments)

    @property
    def tags(self) -> frozenset[str]:
        return frozenset(self._tags)

    def edit(self, title: str, body: str) -> None:
        self._title = title
        self._body = body

    def add_tag(self, tag: str) -> None:
        self._tags.add(tag)

    def add_comment(self, comment: Comment) -> None:
        self._comments.append(comment)

    def remove_comment(self, comment_id: int) -> None:
        self._comments = [c for c in self._comments if c.id != comment_id]
