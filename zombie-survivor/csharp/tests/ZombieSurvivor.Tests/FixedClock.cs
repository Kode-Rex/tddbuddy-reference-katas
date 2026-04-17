namespace ZombieSurvivor.Tests;

public class FixedClock : IClock
{
    private DateTime _now;

    public FixedClock(DateTime now)
    {
        _now = now;
    }

    public DateTime Now() => _now;

    public void AdvanceTo(DateTime dateTime) => _now = dateTime;
}
