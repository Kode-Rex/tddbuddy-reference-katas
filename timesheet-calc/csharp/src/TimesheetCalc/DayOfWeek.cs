namespace TimesheetCalc;

// Our own typed day-of-week enum. Not aliased to System.DayOfWeek because
// the kata's domain vocabulary is the spec — weekdays vs weekend is a
// domain concept, and we want IsWeekend() as a first-class operation.
public enum Day
{
    Monday,
    Tuesday,
    Wednesday,
    Thursday,
    Friday,
    Saturday,
    Sunday,
}

public static class DayExtensions
{
    public static bool IsWeekend(this Day day) =>
        day is Day.Saturday or Day.Sunday;
}
