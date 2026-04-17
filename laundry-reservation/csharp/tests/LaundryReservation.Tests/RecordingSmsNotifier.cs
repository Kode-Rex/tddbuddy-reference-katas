namespace LaundryReservation.Tests;

public record SmsMessage(string To, string Message);

public class RecordingSmsNotifier : ISmsNotifier
{
    private readonly List<SmsMessage> _sent = new();

    public IReadOnlyList<SmsMessage> Sent => _sent;

    public void Send(string to, string message) =>
        _sent.Add(new SmsMessage(to, message));
}
