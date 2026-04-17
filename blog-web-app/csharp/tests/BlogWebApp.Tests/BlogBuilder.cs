using BlogWebApp;

namespace BlogWebApp.Tests;

public class BlogBuilder
{
    private DateTime _startTime = new(2026, 1, 15, 9, 0, 0, DateTimeKind.Utc);
    private readonly List<(string User, string Title, string Body, int MinutesAfterStart)> _seededPosts = new();
    private readonly List<(int PostIndex, string User, string CommentBody, int MinutesAfterStart)> _seededComments = new();
    private readonly List<(int PostIndex, string Tag)> _seededTags = new();

    public BlogBuilder StartingAt(DateTime dateTime)
    {
        _startTime = dateTime;
        return this;
    }

    public BlogBuilder WithPost(string user, string title, string body, int minutesAfterStart = 0)
    {
        _seededPosts.Add((user, title, body, minutesAfterStart));
        return this;
    }

    public BlogBuilder WithComment(int postIndex, string user, string body, int minutesAfterStart = 0)
    {
        _seededComments.Add((postIndex, user, body, minutesAfterStart));
        return this;
    }

    public BlogBuilder WithTag(int postIndex, string tag)
    {
        _seededTags.Add((postIndex, tag));
        return this;
    }

    public (Blog Blog, FixedClock Clock, List<Post> Posts) Build()
    {
        var clock = new FixedClock(_startTime);
        var blog = new Blog(clock);
        var posts = new List<Post>();

        foreach (var (user, title, body, minutesAfterStart) in _seededPosts)
        {
            clock.AdvanceTo(_startTime.AddMinutes(minutesAfterStart));
            posts.Add(blog.CreatePost(user, title, body));
        }

        foreach (var (postIndex, user, body, minutesAfterStart) in _seededComments)
        {
            clock.AdvanceTo(_startTime.AddMinutes(minutesAfterStart));
            blog.AddComment(user, posts[postIndex].Id, body);
        }

        foreach (var (postIndex, tag) in _seededTags)
        {
            blog.AddTag(posts[postIndex].Author, posts[postIndex].Id, tag);
        }

        return (blog, clock, posts);
    }
}
