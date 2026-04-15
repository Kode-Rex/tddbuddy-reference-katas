namespace BankAccount.Tests;

public class FixedClock : IClock
{
    private DateOnly _today;

    public FixedClock(DateOnly today) => _today = today;

    public DateOnly Today() => _today;

    public void AdvanceTo(DateOnly date) => _today = date;
}
