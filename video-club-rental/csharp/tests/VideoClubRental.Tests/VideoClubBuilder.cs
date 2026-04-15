namespace VideoClubRental.Tests;

public class VideoClubBuilder
{
    private DateOnly _openedOn = new(2026, 1, 1);
    private readonly List<User> _users = new();
    private readonly List<Title> _titles = new();

    public VideoClubBuilder OpenedOn(DateOnly date) { _openedOn = date; return this; }

    public VideoClubBuilder WithUser(User user) { _users.Add(user); return this; }

    public VideoClubBuilder WithTitle(Title title) { _titles.Add(title); return this; }

    public (VideoClub Club, RecordingNotifier Notifier, FixedClock Clock) Build()
    {
        var clock = new FixedClock(_openedOn);
        var notifier = new RecordingNotifier();
        var club = new VideoClub(clock, notifier);
        foreach (var user in _users) club.SeedUser(user);
        foreach (var title in _titles) club.AddTitle(title);
        return (club, notifier, clock);
    }
}
