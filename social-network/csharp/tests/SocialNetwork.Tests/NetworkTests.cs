using FluentAssertions;
using SocialNetwork;
using Xunit;

namespace SocialNetwork.Tests;

public class NetworkTests
{
    private static readonly DateTime T0 = new(2026, 1, 15, 9, 0, 0, DateTimeKind.Utc);

    // --- Registration ---

    [Fact]
    public void New_network_has_no_users()
    {
        var (network, _) = new NetworkBuilder().Build();

        network.Users.Should().BeEmpty();
    }

    [Fact]
    public void Posting_auto_registers_a_new_user()
    {
        var (network, _) = new NetworkBuilder()
            .WithPost("Alice", "Hello!")
            .Build();

        network.Users.Should().ContainKey("Alice");
    }

    [Fact]
    public void Posting_with_an_existing_user_does_not_duplicate_registration()
    {
        var (network, _) = new NetworkBuilder()
            .WithPost("Alice", "Hello!", 0)
            .WithPost("Alice", "Again!", 1)
            .Build();

        network.Users.Should().HaveCount(1);
    }

    // --- Posting ---

    [Fact]
    public void A_user_can_post_a_message()
    {
        var (network, _) = new NetworkBuilder()
            .WithPost("Alice", "What a wonderfully sunny day!")
            .Build();

        network.Timeline("Alice").Should().ContainSingle()
            .Which.Content.Should().Be("What a wonderfully sunny day!");
    }

    [Fact]
    public void A_post_records_the_content_and_timestamp_from_the_clock()
    {
        var (network, _) = new NetworkBuilder()
            .StartingAt(T0)
            .WithPost("Alice", "Hello!", 0)
            .Build();

        var post = network.Timeline("Alice").Single();
        post.Author.Should().Be("Alice");
        post.Content.Should().Be("Hello!");
        post.Timestamp.Should().Be(T0);
    }

    [Fact]
    public void A_user_can_post_multiple_messages()
    {
        var (network, _) = new NetworkBuilder()
            .WithPost("Alice", "First", 0)
            .WithPost("Alice", "Second", 1)
            .Build();

        network.Timeline("Alice").Should().HaveCount(2);
    }

    // --- Timeline ---

    [Fact]
    public void Timeline_of_a_user_with_no_posts_is_empty()
    {
        var (network, _) = new NetworkBuilder().Build();

        network.Timeline("Alice").Should().BeEmpty();
    }

    [Fact]
    public void Timeline_returns_the_users_own_posts()
    {
        var (network, _) = new NetworkBuilder()
            .WithPost("Alice", "Sunny day!")
            .Build();

        network.Timeline("Alice").Should().ContainSingle()
            .Which.Content.Should().Be("Sunny day!");
    }

    [Fact]
    public void Timeline_returns_posts_in_reverse_chronological_order()
    {
        var (network, _) = new NetworkBuilder()
            .WithPost("Alice", "First", 0)
            .WithPost("Alice", "Second", 5)
            .WithPost("Alice", "Third", 10)
            .Build();

        var timeline = network.Timeline("Alice");
        timeline[0].Content.Should().Be("Third");
        timeline[1].Content.Should().Be("Second");
        timeline[2].Content.Should().Be("First");
    }

    [Fact]
    public void Timeline_does_not_include_posts_from_other_users()
    {
        var (network, _) = new NetworkBuilder()
            .WithPost("Alice", "Alice's post", 0)
            .WithPost("Bob", "Bob's post", 1)
            .Build();

        network.Timeline("Alice").Should().ContainSingle()
            .Which.Author.Should().Be("Alice");
    }

    // --- Following ---

    [Fact]
    public void A_user_can_follow_another_user()
    {
        var (network, _) = new NetworkBuilder()
            .WithPost("Alice", "Hello!", 0)
            .WithPost("Charlie", "Hi!", 1)
            .WithFollow("Charlie", "Alice")
            .Build();

        network.Users["Charlie"].Following.Should().Contain("Alice");
    }

    [Fact]
    public void Following_is_idempotent()
    {
        var (network, _) = new NetworkBuilder()
            .WithPost("Alice", "Hello!", 0)
            .WithPost("Charlie", "Hi!", 1)
            .WithFollow("Charlie", "Alice")
            .WithFollow("Charlie", "Alice")
            .Build();

        network.Users["Charlie"].Following.Should().HaveCount(1);
    }

    [Fact]
    public void A_user_cannot_follow_themselves()
    {
        var (network, _) = new NetworkBuilder()
            .WithPost("Alice", "Hello!")
            .Build();

        network.Follow("Alice", "Alice");

        network.Users["Alice"].Following.Should().BeEmpty();
    }

    // --- Wall ---

    [Fact]
    public void Wall_of_a_user_with_no_posts_and_no_follows_is_empty()
    {
        var (network, _) = new NetworkBuilder().Build();

        network.Wall("Alice").Should().BeEmpty();
    }

    [Fact]
    public void Wall_shows_the_users_own_posts()
    {
        var (network, _) = new NetworkBuilder()
            .WithPost("Alice", "My post")
            .Build();

        network.Wall("Alice").Should().ContainSingle()
            .Which.Content.Should().Be("My post");
    }

    [Fact]
    public void Wall_includes_posts_from_followed_users()
    {
        var (network, _) = new NetworkBuilder()
            .WithPost("Alice", "Alice's post", 0)
            .WithPost("Bob", "Bob's post", 1)
            .WithPost("Charlie", "Charlie's post", 2)
            .WithFollow("Charlie", "Alice")
            .WithFollow("Charlie", "Bob")
            .Build();

        var wall = network.Wall("Charlie");
        wall.Should().HaveCount(3);
        wall.Select(p => p.Author).Should().Contain("Alice").And.Contain("Bob").And.Contain("Charlie");
    }

    [Fact]
    public void Wall_returns_posts_in_reverse_chronological_order_across_all_authors()
    {
        var (network, _) = new NetworkBuilder()
            .WithPost("Alice", "Alice early", 0)
            .WithPost("Bob", "Bob middle", 5)
            .WithPost("Charlie", "Charlie late", 10)
            .WithFollow("Charlie", "Alice")
            .WithFollow("Charlie", "Bob")
            .Build();

        var wall = network.Wall("Charlie");
        wall[0].Content.Should().Be("Charlie late");
        wall[1].Content.Should().Be("Bob middle");
        wall[2].Content.Should().Be("Alice early");
    }

    [Fact]
    public void Wall_does_not_include_posts_from_users_not_followed()
    {
        var (network, _) = new NetworkBuilder()
            .WithPost("Alice", "Alice's post", 0)
            .WithPost("Bob", "Bob's post", 1)
            .WithPost("Charlie", "Charlie's post", 2)
            .WithFollow("Charlie", "Alice")
            .Build();

        var wall = network.Wall("Charlie");
        wall.Select(p => p.Author).Should().NotContain("Bob");
    }
}
