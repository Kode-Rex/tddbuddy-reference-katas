namespace LaundryReservation;

public interface IEmailNotifier
{
    void Send(string to, string subject, string body);
}
