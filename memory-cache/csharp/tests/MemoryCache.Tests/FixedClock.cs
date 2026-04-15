namespace MemoryCache.Tests;

public class FixedClock : IClock
{
    private DateTime _now;

    public FixedClock(DateTime now) => _now = now;

    public DateTime Now() => _now;

    public void AdvanceTo(DateTime when) => _now = when;

    public void Advance(TimeSpan delta) => _now = _now.Add(delta);
}
