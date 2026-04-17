namespace LaundryReservation;

public interface ISmsNotifier
{
    void Send(string to, string message);
}
