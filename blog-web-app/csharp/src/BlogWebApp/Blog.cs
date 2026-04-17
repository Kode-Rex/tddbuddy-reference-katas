namespace BlogWebApp;

public class Blog
{
    private readonly IClock _clock;
    private readonly Dictionary<string, User> _users = new();
    private readonly Dictionary<int, Post> _posts = new();
    private int _nextPostId = 1;
    private int _nextCommentId = 1;

    public Blog(IClock clock)
    {
        _clock = clock;
    }

    public IReadOnlyDictionary<string, User> Users => _users;
    public IReadOnlyDictionary<int, Post> Posts => _posts;

    public Post CreatePost(string userName, string title, string body)
    {
        EnsureRegistered(userName);
        var post = new Post(_nextPostId++, title, body, userName, _clock.Now());
        _posts[post.Id] = post;
        return post;
    }

    public void EditPost(string userName, int postId, string title, string body)
    {
        var post = GetPostOrThrow(postId);
        EnsureAuthorOfPost(userName, post);
        post.Edit(title, body);
    }

    public void DeletePost(string userName, int postId)
    {
        var post = GetPostOrThrow(postId);
        EnsureAuthorOfPost(userName, post);
        _posts.Remove(postId);
    }

    public Comment AddComment(string userName, int postId, string body)
    {
        var post = GetPostOrThrow(postId);
        EnsureRegistered(userName);
        var comment = new Comment(_nextCommentId++, userName, body, _clock.Now());
        post.AddComment(comment);
        return comment;
    }

    public void DeleteComment(string userName, int postId, int commentId)
    {
        var post = GetPostOrThrow(postId);
        var comment = post.Comments.FirstOrDefault(c => c.Id == commentId);
        if (comment == null) return;

        if (comment.Author != userName)
            throw new UnauthorizedOperationException(
                $"User '{userName}' is not the author of comment '{commentId}'");

        post.RemoveComment(commentId);
    }

    public void AddTag(string userName, int postId, string tag)
    {
        var post = GetPostOrThrow(postId);
        EnsureAuthorOfPost(userName, post);
        post.AddTag(tag);
    }

    public IReadOnlyList<Post> RecentPosts(int count)
    {
        return _posts.Values
            .OrderByDescending(p => p.Timestamp)
            .Take(count)
            .ToList();
    }

    public IReadOnlyList<Post> PostsByTag(string tag)
    {
        return _posts.Values
            .Where(p => p.Tags.Contains(tag))
            .OrderByDescending(p => p.Timestamp)
            .ToList();
    }

    public IReadOnlySet<string> AllTagsForUser(string userName)
    {
        return _posts.Values
            .Where(p => p.Author == userName)
            .SelectMany(p => p.Tags)
            .ToHashSet();
    }

    private void EnsureRegistered(string userName)
    {
        if (!_users.ContainsKey(userName))
            _users[userName] = new User(userName);
    }

    private Post GetPostOrThrow(int postId)
    {
        if (!_posts.TryGetValue(postId, out var post))
            throw new KeyNotFoundException($"Post '{postId}' not found");
        return post;
    }

    private static void EnsureAuthorOfPost(string userName, Post post)
    {
        if (post.Author != userName)
            throw new UnauthorizedOperationException(
                $"User '{userName}' is not the author of post '{post.Id}'");
    }
}
