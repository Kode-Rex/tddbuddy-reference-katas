using SocialNetwork;

namespace SocialNetwork.Tests;

public class NetworkBuilder
{
    private DateTime _startTime = new(2026, 1, 15, 9, 0, 0, DateTimeKind.Utc);
    private readonly List<(string User, string Content, int MinutesAfterStart)> _seededPosts = new();
    private readonly List<(string Follower, string Followee)> _seededFollows = new();

    public NetworkBuilder StartingAt(DateTime dateTime)
    {
        _startTime = dateTime;
        return this;
    }

    public NetworkBuilder WithPost(string user, string content, int minutesAfterStart = 0)
    {
        _seededPosts.Add((user, content, minutesAfterStart));
        return this;
    }

    public NetworkBuilder WithFollow(string follower, string followee)
    {
        _seededFollows.Add((follower, followee));
        return this;
    }

    public (Network Network, FixedClock Clock) Build()
    {
        var clock = new FixedClock(_startTime);
        var network = new Network(clock);

        foreach (var (user, content, minutesAfterStart) in _seededPosts)
        {
            clock.AdvanceTo(_startTime.AddMinutes(minutesAfterStart));
            network.Post(user, content);
        }

        foreach (var (follower, followee) in _seededFollows)
        {
            network.Follow(follower, followee);
        }

        return (network, clock);
    }
}
