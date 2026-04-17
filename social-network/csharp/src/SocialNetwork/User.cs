namespace SocialNetwork;

public class User
{
    public string Name { get; }
    private readonly HashSet<string> _following = new();

    public User(string name)
    {
        Name = name;
    }

    public IReadOnlySet<string> Following => _following;

    public bool Follow(string userName)
    {
        if (userName == Name) return false;
        return _following.Add(userName);
    }
}
