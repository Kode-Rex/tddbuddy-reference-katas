namespace LaundryReservation.Tests;

public record EmailMessage(string To, string Subject, string Body);

public class RecordingEmailNotifier : IEmailNotifier
{
    private readonly List<EmailMessage> _sent = new();

    public IReadOnlyList<EmailMessage> Sent => _sent;

    public void Send(string to, string subject, string body) =>
        _sent.Add(new EmailMessage(to, subject, body));
}
