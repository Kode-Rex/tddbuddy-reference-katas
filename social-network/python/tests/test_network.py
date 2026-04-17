from datetime import datetime, timezone

from .network_builder import NetworkBuilder

T0 = datetime(2026, 1, 15, 9, 0, 0, tzinfo=timezone.utc)


# --- Registration ---


def test_new_network_has_no_users():
    network, _ = NetworkBuilder().build()
    assert network.users == {}


def test_posting_auto_registers_a_new_user():
    network, _ = NetworkBuilder().with_post("Alice", "Hello!").build()
    assert "Alice" in network.users


def test_posting_with_an_existing_user_does_not_duplicate_registration():
    network, _ = (
        NetworkBuilder()
        .with_post("Alice", "Hello!", 0)
        .with_post("Alice", "Again!", 1)
        .build()
    )
    assert len(network.users) == 1


# --- Posting ---


def test_a_user_can_post_a_message():
    network, _ = (
        NetworkBuilder()
        .with_post("Alice", "What a wonderfully sunny day!")
        .build()
    )
    timeline = network.timeline("Alice")
    assert len(timeline) == 1
    assert timeline[0].content == "What a wonderfully sunny day!"


def test_a_post_records_the_content_and_timestamp_from_the_clock():
    network, _ = (
        NetworkBuilder()
        .starting_at(T0)
        .with_post("Alice", "Hello!", 0)
        .build()
    )
    post = network.timeline("Alice")[0]
    assert post.author == "Alice"
    assert post.content == "Hello!"
    assert post.timestamp == T0


def test_a_user_can_post_multiple_messages():
    network, _ = (
        NetworkBuilder()
        .with_post("Alice", "First", 0)
        .with_post("Alice", "Second", 1)
        .build()
    )
    assert len(network.timeline("Alice")) == 2


# --- Timeline ---


def test_timeline_of_a_user_with_no_posts_is_empty():
    network, _ = NetworkBuilder().build()
    assert network.timeline("Alice") == []


def test_timeline_returns_the_users_own_posts():
    network, _ = NetworkBuilder().with_post("Alice", "Sunny day!").build()
    timeline = network.timeline("Alice")
    assert len(timeline) == 1
    assert timeline[0].content == "Sunny day!"


def test_timeline_returns_posts_in_reverse_chronological_order():
    network, _ = (
        NetworkBuilder()
        .with_post("Alice", "First", 0)
        .with_post("Alice", "Second", 5)
        .with_post("Alice", "Third", 10)
        .build()
    )
    timeline = network.timeline("Alice")
    assert timeline[0].content == "Third"
    assert timeline[1].content == "Second"
    assert timeline[2].content == "First"


def test_timeline_does_not_include_posts_from_other_users():
    network, _ = (
        NetworkBuilder()
        .with_post("Alice", "Alice's post", 0)
        .with_post("Bob", "Bob's post", 1)
        .build()
    )
    timeline = network.timeline("Alice")
    assert len(timeline) == 1
    assert timeline[0].author == "Alice"


# --- Following ---


def test_a_user_can_follow_another_user():
    network, _ = (
        NetworkBuilder()
        .with_post("Alice", "Hello!", 0)
        .with_post("Charlie", "Hi!", 1)
        .with_follow("Charlie", "Alice")
        .build()
    )
    assert "Alice" in network.users["Charlie"].following


def test_following_is_idempotent():
    network, _ = (
        NetworkBuilder()
        .with_post("Alice", "Hello!", 0)
        .with_post("Charlie", "Hi!", 1)
        .with_follow("Charlie", "Alice")
        .with_follow("Charlie", "Alice")
        .build()
    )
    assert len(network.users["Charlie"].following) == 1


def test_a_user_cannot_follow_themselves():
    network, _ = NetworkBuilder().with_post("Alice", "Hello!").build()
    network.follow("Alice", "Alice")
    assert len(network.users["Alice"].following) == 0


# --- Wall ---


def test_wall_of_a_user_with_no_posts_and_no_follows_is_empty():
    network, _ = NetworkBuilder().build()
    assert network.wall("Alice") == []


def test_wall_shows_the_users_own_posts():
    network, _ = NetworkBuilder().with_post("Alice", "My post").build()
    wall = network.wall("Alice")
    assert len(wall) == 1
    assert wall[0].content == "My post"


def test_wall_includes_posts_from_followed_users():
    network, _ = (
        NetworkBuilder()
        .with_post("Alice", "Alice's post", 0)
        .with_post("Bob", "Bob's post", 1)
        .with_post("Charlie", "Charlie's post", 2)
        .with_follow("Charlie", "Alice")
        .with_follow("Charlie", "Bob")
        .build()
    )
    wall = network.wall("Charlie")
    assert len(wall) == 3
    authors = {p.author for p in wall}
    assert authors == {"Alice", "Bob", "Charlie"}


def test_wall_returns_posts_in_reverse_chronological_order_across_all_authors():
    network, _ = (
        NetworkBuilder()
        .with_post("Alice", "Alice early", 0)
        .with_post("Bob", "Bob middle", 5)
        .with_post("Charlie", "Charlie late", 10)
        .with_follow("Charlie", "Alice")
        .with_follow("Charlie", "Bob")
        .build()
    )
    wall = network.wall("Charlie")
    assert wall[0].content == "Charlie late"
    assert wall[1].content == "Bob middle"
    assert wall[2].content == "Alice early"


def test_wall_does_not_include_posts_from_users_not_followed():
    network, _ = (
        NetworkBuilder()
        .with_post("Alice", "Alice's post", 0)
        .with_post("Bob", "Bob's post", 1)
        .with_post("Charlie", "Charlie's post", 2)
        .with_follow("Charlie", "Alice")
        .build()
    )
    wall = network.wall("Charlie")
    authors = {p.author for p in wall}
    assert "Bob" not in authors
