namespace TimesheetCalc;

public sealed class Timesheet
{
    private readonly IReadOnlyDictionary<Day, double> _entries;

    public Timesheet(IReadOnlyDictionary<Day, double> entries)
    {
        foreach (var hours in entries.Values)
        {
            if (hours < 0)
            {
                throw new ArgumentException(ErrorMessages.HoursMustNotBeNegative);
            }
        }
        _entries = entries;
    }

    public TimesheetTotals Totals()
    {
        double regular = 0;
        double overtime = 0;

        foreach (var (day, hours) in _entries)
        {
            if (day.IsWeekend())
            {
                overtime += hours;
            }
            else if (hours > OvertimeRules.DailyOvertimeThreshold)
            {
                regular += OvertimeRules.DailyOvertimeThreshold;
                overtime += hours - OvertimeRules.DailyOvertimeThreshold;
            }
            else
            {
                regular += hours;
            }
        }

        return new TimesheetTotals(regular, overtime);
    }
}
