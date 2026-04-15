namespace LastSunday;

public static class LastSunday
{
    public static DateOnly Find(int year, int month)
    {
        var lastDay = new DateOnly(year, month, DateTime.DaysInMonth(year, month));
        var daysToWalkBack = ((int)lastDay.DayOfWeek - (int)DayOfWeek.Sunday + 7) % 7;
        return lastDay.AddDays(-daysToWalkBack);
    }
}
