using FluentAssertions;
using BlogWebApp;
using Xunit;

namespace BlogWebApp.Tests;

public class BlogTests
{
    private static readonly DateTime T0 = new(2026, 1, 15, 9, 0, 0, DateTimeKind.Utc);

    // --- Post Creation ---

    [Fact]
    public void New_blog_has_no_posts()
    {
        var (blog, _, _) = new BlogBuilder().Build();

        blog.Posts.Should().BeEmpty();
    }

    [Fact]
    public void Creating_a_post_auto_registers_the_user()
    {
        var (blog, _, _) = new BlogBuilder()
            .WithPost("Alice", "Hello", "World")
            .Build();

        blog.Users.Should().ContainKey("Alice");
    }

    [Fact]
    public void A_post_records_title_body_author_and_timestamp()
    {
        var (blog, _, posts) = new BlogBuilder()
            .StartingAt(T0)
            .WithPost("Alice", "My Title", "My Body", 0)
            .Build();

        var post = posts[0];
        post.Title.Should().Be("My Title");
        post.Body.Should().Be("My Body");
        post.Author.Should().Be("Alice");
        post.Timestamp.Should().Be(T0);
    }

    [Fact]
    public void A_user_can_create_multiple_posts()
    {
        var (blog, _, _) = new BlogBuilder()
            .WithPost("Alice", "First", "Body 1", 0)
            .WithPost("Alice", "Second", "Body 2", 1)
            .Build();

        blog.Posts.Should().HaveCount(2);
    }

    [Fact]
    public void Each_post_receives_a_unique_id()
    {
        var (_, _, posts) = new BlogBuilder()
            .WithPost("Alice", "First", "Body 1", 0)
            .WithPost("Alice", "Second", "Body 2", 1)
            .Build();

        posts[0].Id.Should().NotBe(posts[1].Id);
    }

    // --- Editing Posts ---

    [Fact]
    public void A_user_can_edit_their_own_post()
    {
        var (blog, _, posts) = new BlogBuilder()
            .WithPost("Alice", "Original", "Original body")
            .Build();

        blog.EditPost("Alice", posts[0].Id, "Updated", "Updated body");

        posts[0].Title.Should().Be("Updated");
    }

    [Fact]
    public void Editing_updates_both_title_and_body()
    {
        var (blog, _, posts) = new BlogBuilder()
            .WithPost("Alice", "Original", "Original body")
            .Build();

        blog.EditPost("Alice", posts[0].Id, "New Title", "New Body");

        posts[0].Title.Should().Be("New Title");
        posts[0].Body.Should().Be("New Body");
    }

    [Fact]
    public void Editing_another_users_post_throws_unauthorized()
    {
        var (blog, _, posts) = new BlogBuilder()
            .WithPost("Alice", "Alice's Post", "Body")
            .WithPost("Bob", "Bob's Post", "Body", 1)
            .Build();

        var act = () => blog.EditPost("Bob", posts[0].Id, "Hacked", "Hacked");

        act.Should().Throw<UnauthorizedOperationException>()
            .WithMessage("User 'Bob' is not the author of post '1'");
    }

    // --- Deleting Posts ---

    [Fact]
    public void A_user_can_delete_their_own_post()
    {
        var (blog, _, posts) = new BlogBuilder()
            .WithPost("Alice", "To Delete", "Body")
            .Build();

        blog.DeletePost("Alice", posts[0].Id);

        blog.Posts.Should().BeEmpty();
    }

    [Fact]
    public void Deleting_another_users_post_throws_unauthorized()
    {
        var (blog, _, posts) = new BlogBuilder()
            .WithPost("Alice", "Alice's Post", "Body")
            .WithPost("Bob", "Bob's Post", "Body", 1)
            .Build();

        var act = () => blog.DeletePost("Bob", posts[0].Id);

        act.Should().Throw<UnauthorizedOperationException>()
            .WithMessage("User 'Bob' is not the author of post '1'");
    }

    [Fact]
    public void Deleting_a_post_removes_its_comments()
    {
        var (blog, _, posts) = new BlogBuilder()
            .WithPost("Alice", "Post", "Body")
            .WithComment(0, "Bob", "Nice post!", 1)
            .Build();

        blog.DeletePost("Alice", posts[0].Id);

        blog.Posts.Should().BeEmpty();
    }

    // --- Comments ---

    [Fact]
    public void Any_user_can_comment_on_any_post()
    {
        var (blog, _, posts) = new BlogBuilder()
            .WithPost("Alice", "Post", "Body")
            .WithComment(0, "Bob", "Great post!", 1)
            .Build();

        posts[0].Comments.Should().ContainSingle()
            .Which.Body.Should().Be("Great post!");
    }

    [Fact]
    public void A_comment_records_author_body_and_timestamp()
    {
        var (blog, _, posts) = new BlogBuilder()
            .StartingAt(T0)
            .WithPost("Alice", "Post", "Body", 0)
            .WithComment(0, "Bob", "Nice!", 5)
            .Build();

        var comment = posts[0].Comments.Single();
        comment.Author.Should().Be("Bob");
        comment.Body.Should().Be("Nice!");
        comment.Timestamp.Should().Be(T0.AddMinutes(5));
    }

    [Fact]
    public void Commenting_auto_registers_the_user()
    {
        var (blog, _, _) = new BlogBuilder()
            .WithPost("Alice", "Post", "Body")
            .WithComment(0, "Charlie", "Hello!", 1)
            .Build();

        blog.Users.Should().ContainKey("Charlie");
    }

    [Fact]
    public void A_user_can_delete_their_own_comment()
    {
        var (blog, _, posts) = new BlogBuilder()
            .WithPost("Alice", "Post", "Body")
            .WithComment(0, "Bob", "To delete", 1)
            .Build();

        var commentId = posts[0].Comments.Single().Id;
        blog.DeleteComment("Bob", posts[0].Id, commentId);

        posts[0].Comments.Should().BeEmpty();
    }

    [Fact]
    public void Deleting_another_users_comment_throws_unauthorized()
    {
        var (blog, _, posts) = new BlogBuilder()
            .WithPost("Alice", "Post", "Body")
            .WithComment(0, "Bob", "Bob's comment", 1)
            .Build();

        var commentId = posts[0].Comments.Single().Id;
        var act = () => blog.DeleteComment("Alice", posts[0].Id, commentId);

        act.Should().Throw<UnauthorizedOperationException>()
            .WithMessage($"User 'Alice' is not the author of comment '{commentId}'");
    }

    // --- Tags ---

    [Fact]
    public void A_post_author_can_add_a_tag_to_their_own_post()
    {
        var (blog, _, posts) = new BlogBuilder()
            .WithPost("Alice", "TDD Post", "Body")
            .WithTag(0, "TDD")
            .Build();

        posts[0].Tags.Should().Contain("TDD");
    }

    [Fact]
    public void Adding_a_tag_to_another_users_post_throws_unauthorized()
    {
        var (blog, _, posts) = new BlogBuilder()
            .WithPost("Alice", "Post", "Body")
            .WithPost("Bob", "Bob's Post", "Body", 1)
            .Build();

        var act = () => blog.AddTag("Bob", posts[0].Id, "hack");

        act.Should().Throw<UnauthorizedOperationException>()
            .WithMessage("User 'Bob' is not the author of post '1'");
    }

    [Fact]
    public void Adding_the_same_tag_twice_is_idempotent()
    {
        var (blog, _, posts) = new BlogBuilder()
            .WithPost("Alice", "Post", "Body")
            .WithTag(0, "TDD")
            .WithTag(0, "TDD")
            .Build();

        posts[0].Tags.Should().HaveCount(1);
    }

    // --- Queries ---

    [Fact]
    public void Recent_posts_returns_the_N_most_recent_posts()
    {
        var (blog, _, _) = new BlogBuilder()
            .WithPost("Alice", "First", "Body", 0)
            .WithPost("Bob", "Second", "Body", 5)
            .WithPost("Alice", "Third", "Body", 10)
            .WithPost("Bob", "Fourth", "Body", 15)
            .WithPost("Alice", "Fifth", "Body", 20)
            .Build();

        var recent = blog.RecentPosts(3);
        recent.Should().HaveCount(3);
        recent[0].Title.Should().Be("Fifth");
        recent[1].Title.Should().Be("Fourth");
        recent[2].Title.Should().Be("Third");
    }

    [Fact]
    public void Recent_posts_returns_fewer_than_N_when_not_enough_posts_exist()
    {
        var (blog, _, _) = new BlogBuilder()
            .WithPost("Alice", "Only", "Body")
            .Build();

        var recent = blog.RecentPosts(5);
        recent.Should().HaveCount(1);
    }

    [Fact]
    public void Posts_by_tag_returns_matching_posts_most_recent_first()
    {
        var (blog, _, posts) = new BlogBuilder()
            .WithPost("Alice", "First TDD", "Body", 0)
            .WithPost("Bob", "Second TDD", "Body", 5)
            .WithPost("Alice", "No Tag", "Body", 10)
            .WithTag(0, "TDD")
            .WithTag(1, "TDD")
            .Build();

        var results = blog.PostsByTag("TDD");
        results.Should().HaveCount(2);
        results[0].Title.Should().Be("Second TDD");
        results[1].Title.Should().Be("First TDD");
    }

    [Fact]
    public void Posts_by_tag_returns_empty_when_no_posts_match()
    {
        var (blog, _, _) = new BlogBuilder()
            .WithPost("Alice", "Post", "Body")
            .Build();

        blog.PostsByTag("nonexistent").Should().BeEmpty();
    }

    [Fact]
    public void All_tags_for_user_returns_distinct_tags_across_their_posts()
    {
        var (blog, _, _) = new BlogBuilder()
            .WithPost("Alice", "Post 1", "Body", 0)
            .WithPost("Alice", "Post 2", "Body", 1)
            .WithTag(0, "TDD")
            .WithTag(0, "C#")
            .WithTag(1, "TDD")
            .WithTag(1, "Testing")
            .Build();

        var tags = blog.AllTagsForUser("Alice");
        tags.Should().BeEquivalentTo(new[] { "TDD", "C#", "Testing" });
    }

    [Fact]
    public void All_tags_for_user_returns_empty_when_user_has_no_tags()
    {
        var (blog, _, _) = new BlogBuilder()
            .WithPost("Alice", "Post", "Body")
            .Build();

        blog.AllTagsForUser("Alice").Should().BeEmpty();
    }
}
