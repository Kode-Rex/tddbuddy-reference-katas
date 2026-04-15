namespace TimesheetCalc;

public sealed record TimesheetTotals(double RegularHours, double OvertimeHours)
{
    public double TotalHours => RegularHours + OvertimeHours;
}
