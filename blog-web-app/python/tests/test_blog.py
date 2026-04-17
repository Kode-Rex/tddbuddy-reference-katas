from datetime import datetime, timedelta, timezone

import pytest

from blog_web_app import UnauthorizedOperationError

from .blog_builder import BlogBuilder

T0 = datetime(2026, 1, 15, 9, 0, 0, tzinfo=timezone.utc)


# --- Post Creation ---


def test_new_blog_has_no_posts():
    blog, _, _ = BlogBuilder().build()
    assert blog.posts == {}


def test_creating_a_post_auto_registers_the_user():
    blog, _, _ = BlogBuilder().with_post("Alice", "Hello", "World").build()
    assert "Alice" in blog.users


def test_a_post_records_title_body_author_and_timestamp():
    _, _, posts = (
        BlogBuilder().starting_at(T0).with_post("Alice", "My Title", "My Body", 0).build()
    )
    post = posts[0]
    assert post.title == "My Title"
    assert post.body == "My Body"
    assert post.author == "Alice"
    assert post.timestamp == T0


def test_a_user_can_create_multiple_posts():
    blog, _, _ = (
        BlogBuilder()
        .with_post("Alice", "First", "Body 1", 0)
        .with_post("Alice", "Second", "Body 2", 1)
        .build()
    )
    assert len(blog.posts) == 2


def test_each_post_receives_a_unique_id():
    _, _, posts = (
        BlogBuilder()
        .with_post("Alice", "First", "Body 1", 0)
        .with_post("Alice", "Second", "Body 2", 1)
        .build()
    )
    assert posts[0].id != posts[1].id


# --- Editing Posts ---


def test_a_user_can_edit_their_own_post():
    blog, _, posts = BlogBuilder().with_post("Alice", "Original", "Original body").build()
    blog.edit_post("Alice", posts[0].id, "Updated", "Updated body")
    assert posts[0].title == "Updated"


def test_editing_updates_both_title_and_body():
    blog, _, posts = BlogBuilder().with_post("Alice", "Original", "Original body").build()
    blog.edit_post("Alice", posts[0].id, "New Title", "New Body")
    assert posts[0].title == "New Title"
    assert posts[0].body == "New Body"


def test_editing_another_users_post_throws_unauthorized():
    blog, _, posts = (
        BlogBuilder()
        .with_post("Alice", "Alice's Post", "Body")
        .with_post("Bob", "Bob's Post", "Body", 1)
        .build()
    )
    with pytest.raises(
        UnauthorizedOperationError,
        match="User 'Bob' is not the author of post '1'",
    ):
        blog.edit_post("Bob", posts[0].id, "Hacked", "Hacked")


# --- Deleting Posts ---


def test_a_user_can_delete_their_own_post():
    blog, _, posts = BlogBuilder().with_post("Alice", "To Delete", "Body").build()
    blog.delete_post("Alice", posts[0].id)
    assert blog.posts == {}


def test_deleting_another_users_post_throws_unauthorized():
    blog, _, posts = (
        BlogBuilder()
        .with_post("Alice", "Alice's Post", "Body")
        .with_post("Bob", "Bob's Post", "Body", 1)
        .build()
    )
    with pytest.raises(
        UnauthorizedOperationError,
        match="User 'Bob' is not the author of post '1'",
    ):
        blog.delete_post("Bob", posts[0].id)


def test_deleting_a_post_removes_its_comments():
    blog, _, posts = (
        BlogBuilder()
        .with_post("Alice", "Post", "Body")
        .with_comment(0, "Bob", "Nice post!", 1)
        .build()
    )
    blog.delete_post("Alice", posts[0].id)
    assert blog.posts == {}


# --- Comments ---


def test_any_user_can_comment_on_any_post():
    _, _, posts = (
        BlogBuilder()
        .with_post("Alice", "Post", "Body")
        .with_comment(0, "Bob", "Great post!", 1)
        .build()
    )
    assert len(posts[0].comments) == 1
    assert posts[0].comments[0].body == "Great post!"


def test_a_comment_records_author_body_and_timestamp():
    _, _, posts = (
        BlogBuilder()
        .starting_at(T0)
        .with_post("Alice", "Post", "Body", 0)
        .with_comment(0, "Bob", "Nice!", 5)
        .build()
    )
    comment = posts[0].comments[0]
    assert comment.author == "Bob"
    assert comment.body == "Nice!"
    assert comment.timestamp == T0 + timedelta(minutes=5)


def test_commenting_auto_registers_the_user():
    blog, _, _ = (
        BlogBuilder()
        .with_post("Alice", "Post", "Body")
        .with_comment(0, "Charlie", "Hello!", 1)
        .build()
    )
    assert "Charlie" in blog.users


def test_a_user_can_delete_their_own_comment():
    blog, _, posts = (
        BlogBuilder()
        .with_post("Alice", "Post", "Body")
        .with_comment(0, "Bob", "To delete", 1)
        .build()
    )
    comment_id = posts[0].comments[0].id
    blog.delete_comment("Bob", posts[0].id, comment_id)
    assert len(posts[0].comments) == 0


def test_deleting_another_users_comment_throws_unauthorized():
    blog, _, posts = (
        BlogBuilder()
        .with_post("Alice", "Post", "Body")
        .with_comment(0, "Bob", "Bob's comment", 1)
        .build()
    )
    comment_id = posts[0].comments[0].id
    with pytest.raises(
        UnauthorizedOperationError,
        match=f"User 'Alice' is not the author of comment '{comment_id}'",
    ):
        blog.delete_comment("Alice", posts[0].id, comment_id)


# --- Tags ---


def test_a_post_author_can_add_a_tag_to_their_own_post():
    _, _, posts = (
        BlogBuilder()
        .with_post("Alice", "TDD Post", "Body")
        .with_tag(0, "TDD")
        .build()
    )
    assert "TDD" in posts[0].tags


def test_adding_a_tag_to_another_users_post_throws_unauthorized():
    blog, _, posts = (
        BlogBuilder()
        .with_post("Alice", "Post", "Body")
        .with_post("Bob", "Bob's Post", "Body", 1)
        .build()
    )
    with pytest.raises(
        UnauthorizedOperationError,
        match="User 'Bob' is not the author of post '1'",
    ):
        blog.add_tag("Bob", posts[0].id, "hack")


def test_adding_the_same_tag_twice_is_idempotent():
    _, _, posts = (
        BlogBuilder()
        .with_post("Alice", "Post", "Body")
        .with_tag(0, "TDD")
        .with_tag(0, "TDD")
        .build()
    )
    assert len(posts[0].tags) == 1


# --- Queries ---


def test_recent_posts_returns_the_n_most_recent_posts():
    blog, _, _ = (
        BlogBuilder()
        .with_post("Alice", "First", "Body", 0)
        .with_post("Bob", "Second", "Body", 5)
        .with_post("Alice", "Third", "Body", 10)
        .with_post("Bob", "Fourth", "Body", 15)
        .with_post("Alice", "Fifth", "Body", 20)
        .build()
    )
    recent = blog.recent_posts(3)
    assert len(recent) == 3
    assert recent[0].title == "Fifth"
    assert recent[1].title == "Fourth"
    assert recent[2].title == "Third"


def test_recent_posts_returns_fewer_than_n_when_not_enough_posts_exist():
    blog, _, _ = BlogBuilder().with_post("Alice", "Only", "Body").build()
    recent = blog.recent_posts(5)
    assert len(recent) == 1


def test_posts_by_tag_returns_matching_posts_most_recent_first():
    blog, _, _ = (
        BlogBuilder()
        .with_post("Alice", "First TDD", "Body", 0)
        .with_post("Bob", "Second TDD", "Body", 5)
        .with_post("Alice", "No Tag", "Body", 10)
        .with_tag(0, "TDD")
        .with_tag(1, "TDD")
        .build()
    )
    results = blog.posts_by_tag("TDD")
    assert len(results) == 2
    assert results[0].title == "Second TDD"
    assert results[1].title == "First TDD"


def test_posts_by_tag_returns_empty_when_no_posts_match():
    blog, _, _ = BlogBuilder().with_post("Alice", "Post", "Body").build()
    assert blog.posts_by_tag("nonexistent") == []


def test_all_tags_for_user_returns_distinct_tags_across_their_posts():
    blog, _, _ = (
        BlogBuilder()
        .with_post("Alice", "Post 1", "Body", 0)
        .with_post("Alice", "Post 2", "Body", 1)
        .with_tag(0, "TDD")
        .with_tag(0, "C#")
        .with_tag(1, "TDD")
        .with_tag(1, "Testing")
        .build()
    )
    tags = blog.all_tags_for_user("Alice")
    assert tags == frozenset({"TDD", "C#", "Testing"})


def test_all_tags_for_user_returns_empty_when_user_has_no_tags():
    blog, _, _ = BlogBuilder().with_post("Alice", "Post", "Body").build()
    assert blog.all_tags_for_user("Alice") == frozenset()
