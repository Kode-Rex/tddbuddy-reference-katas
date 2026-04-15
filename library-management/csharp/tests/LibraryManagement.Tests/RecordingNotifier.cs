namespace LibraryManagement.Tests;

public record Notification(Member Member, string Message);

public class RecordingNotifier : INotifier
{
    private readonly List<Notification> _sent = new();

    public IReadOnlyList<Notification> Sent => _sent;

    public void Send(Member member, string message) => _sent.Add(new Notification(member, message));

    public IReadOnlyList<Notification> NotificationsFor(Member member) =>
        _sent.Where(n => ReferenceEquals(n.Member, member)).ToList();

    public IReadOnlyList<Notification> AvailabilityNotificationsFor(Member member) =>
        NotificationsFor(member).Where(n => n.Message.Contains("available")).ToList();

    public IReadOnlyList<Notification> ExpirationNotificationsFor(Member member) =>
        NotificationsFor(member).Where(n => n.Message.Contains("expired")).ToList();
}
