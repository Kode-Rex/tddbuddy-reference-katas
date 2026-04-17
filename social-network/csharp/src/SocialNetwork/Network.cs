namespace SocialNetwork;

public class Network
{
    private readonly IClock _clock;
    private readonly Dictionary<string, User> _users = new();
    private readonly List<Post> _posts = new();

    public Network(IClock clock)
    {
        _clock = clock;
    }

    public IReadOnlyDictionary<string, User> Users => _users;

    public void Post(string userName, string content)
    {
        EnsureRegistered(userName);
        _posts.Add(new Post(userName, content, _clock.Now()));
    }

    public void Follow(string followerName, string followeeName)
    {
        EnsureRegistered(followerName);
        EnsureRegistered(followeeName);
        _users[followerName].Follow(followeeName);
    }

    public IReadOnlyList<Post> Timeline(string userName)
    {
        return _posts
            .Where(p => p.Author == userName)
            .OrderByDescending(p => p.Timestamp)
            .ToList();
    }

    public IReadOnlyList<Post> Wall(string userName)
    {
        var user = _users.GetValueOrDefault(userName);
        if (user == null) return Array.Empty<Post>();

        var visible = new HashSet<string> { userName };
        foreach (var followed in user.Following)
            visible.Add(followed);

        return _posts
            .Where(p => visible.Contains(p.Author))
            .OrderByDescending(p => p.Timestamp)
            .ToList();
    }

    private void EnsureRegistered(string userName)
    {
        if (!_users.ContainsKey(userName))
            _users[userName] = new User(userName);
    }
}
