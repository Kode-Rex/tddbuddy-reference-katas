namespace LibraryManagement.Tests;

public class FixedClock : IClock
{
    private DateOnly _today;

    public FixedClock(DateOnly today) => _today = today;

    public DateOnly Today() => _today;

    public void AdvanceTo(DateOnly date) => _today = date;

    public void AdvanceDays(int days) => _today = _today.AddDays(days);
}
