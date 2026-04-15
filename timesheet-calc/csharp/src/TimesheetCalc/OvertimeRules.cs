namespace TimesheetCalc;

// Business numbers are named. F2 is Full-Bake — named constants win.
// Identical values across C#, TypeScript, and Python.
public static class OvertimeRules
{
    public const double DailyOvertimeThreshold = 8;
    public const double StandardWorkWeekHours = 40;
}

// The error message string is the spec — identical byte-for-byte across
// all three languages. The exception type is language-idiomatic.
public static class ErrorMessages
{
    public const string HoursMustNotBeNegative = "hours must not be negative";
}
