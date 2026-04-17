from __future__ import annotations

from blog_web_app.clock import Clock
from blog_web_app.comment import Comment
from blog_web_app.exceptions import UnauthorizedOperationError
from blog_web_app.post import Post
from blog_web_app.user import User


class Blog:
    def __init__(self, clock: Clock) -> None:
        self._clock = clock
        self._users: dict[str, User] = {}
        self._posts: dict[int, Post] = {}
        self._next_post_id = 1
        self._next_comment_id = 1

    @property
    def users(self) -> dict[str, User]:
        return dict(self._users)

    @property
    def posts(self) -> dict[int, Post]:
        return dict(self._posts)

    def create_post(self, user_name: str, title: str, body: str) -> Post:
        self._ensure_registered(user_name)
        post = Post(self._next_post_id, title, body, user_name, self._clock.now())
        self._next_post_id += 1
        self._posts[post.id] = post
        return post

    def edit_post(self, user_name: str, post_id: int, title: str, body: str) -> None:
        post = self._get_post_or_throw(post_id)
        self._ensure_author_of_post(user_name, post)
        post.edit(title, body)

    def delete_post(self, user_name: str, post_id: int) -> None:
        post = self._get_post_or_throw(post_id)
        self._ensure_author_of_post(user_name, post)
        del self._posts[post_id]

    def add_comment(self, user_name: str, post_id: int, body: str) -> Comment:
        post = self._get_post_or_throw(post_id)
        self._ensure_registered(user_name)
        comment = Comment(self._next_comment_id, user_name, body, self._clock.now())
        self._next_comment_id += 1
        post.add_comment(comment)
        return comment

    def delete_comment(self, user_name: str, post_id: int, comment_id: int) -> None:
        post = self._get_post_or_throw(post_id)
        comment = next((c for c in post.comments if c.id == comment_id), None)
        if comment is None:
            return

        if comment.author != user_name:
            raise UnauthorizedOperationError(
                f"User '{user_name}' is not the author of comment '{comment_id}'"
            )

        post.remove_comment(comment_id)

    def add_tag(self, user_name: str, post_id: int, tag: str) -> None:
        post = self._get_post_or_throw(post_id)
        self._ensure_author_of_post(user_name, post)
        post.add_tag(tag)

    def recent_posts(self, count: int) -> list[Post]:
        return sorted(
            self._posts.values(),
            key=lambda p: p.timestamp,
            reverse=True,
        )[:count]

    def posts_by_tag(self, tag: str) -> list[Post]:
        return sorted(
            [p for p in self._posts.values() if tag in p.tags],
            key=lambda p: p.timestamp,
            reverse=True,
        )

    def all_tags_for_user(self, user_name: str) -> frozenset[str]:
        tags: set[str] = set()
        for post in self._posts.values():
            if post.author == user_name:
                tags.update(post.tags)
        return frozenset(tags)

    def _ensure_registered(self, user_name: str) -> None:
        if user_name not in self._users:
            self._users[user_name] = User(user_name)

    def _get_post_or_throw(self, post_id: int) -> Post:
        post = self._posts.get(post_id)
        if post is None:
            raise KeyError(f"Post '{post_id}' not found")
        return post

    @staticmethod
    def _ensure_author_of_post(user_name: str, post: Post) -> None:
        if post.author != user_name:
            raise UnauthorizedOperationError(
                f"User '{user_name}' is not the author of post '{post.id}'"
            )
