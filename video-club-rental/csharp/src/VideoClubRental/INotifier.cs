namespace VideoClubRental;

public interface INotifier
{
    void Send(User user, string message);
}
