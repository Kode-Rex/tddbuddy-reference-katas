using BlogWebApp;

namespace BlogWebApp.Tests;

public class FixedClock : IClock
{
    private DateTime _current;

    public FixedClock(DateTime current)
    {
        _current = current;
    }

    public DateTime Now() => _current;

    public void AdvanceTo(DateTime dateTime) => _current = dateTime;

    public void AdvanceByMinutes(int minutes) => _current = _current.AddMinutes(minutes);
}
