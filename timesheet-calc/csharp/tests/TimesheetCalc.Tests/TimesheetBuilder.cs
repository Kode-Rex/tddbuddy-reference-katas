namespace TimesheetCalc.Tests;

public class TimesheetBuilder
{
    private readonly Dictionary<Day, double> _entries = new();

    public TimesheetBuilder WithEntry(Day day, double hours)
    {
        _entries[day] = hours;
        return this;
    }

    public Timesheet Build() => new(_entries);
}
