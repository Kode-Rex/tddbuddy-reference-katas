namespace VideoClubRental.Tests;

public record Notification(User User, string Message);

public class RecordingNotifier : INotifier
{
    private readonly List<Notification> _sent = new();

    public IReadOnlyList<Notification> Sent => _sent;

    public void Send(User user, string message) => _sent.Add(new Notification(user, message));

    public IReadOnlyList<Notification> NotificationsFor(User user) =>
        _sent.Where(n => ReferenceEquals(n.User, user)).ToList();
}
