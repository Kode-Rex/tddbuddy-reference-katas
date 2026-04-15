namespace LibraryManagement;

public interface INotifier
{
    void Send(Member member, string message);
}
