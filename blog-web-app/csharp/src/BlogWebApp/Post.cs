namespace BlogWebApp;

public class Post
{
    private readonly List<Comment> _comments = new();
    private readonly HashSet<string> _tags = new();

    public int Id { get; }
    public string Title { get; private set; }
    public string Body { get; private set; }
    public string Author { get; }
    public DateTime Timestamp { get; }

    public IReadOnlyList<Comment> Comments => _comments;
    public IReadOnlySet<string> Tags => _tags;

    public Post(int id, string title, string body, string author, DateTime timestamp)
    {
        Id = id;
        Title = title;
        Body = body;
        Author = author;
        Timestamp = timestamp;
    }

    internal void Edit(string title, string body)
    {
        Title = title;
        Body = body;
    }

    internal void AddTag(string tag)
    {
        _tags.Add(tag);
    }

    internal void AddComment(Comment comment)
    {
        _comments.Add(comment);
    }

    internal void RemoveComment(int commentId)
    {
        _comments.RemoveAll(c => c.Id == commentId);
    }
}
