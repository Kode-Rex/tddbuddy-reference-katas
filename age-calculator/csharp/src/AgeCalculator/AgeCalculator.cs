namespace AgeCalculator;

public static class AgeCalculator
{
    public static int Calculate(DateOnly birthdate, DateOnly today)
    {
        if (birthdate > today)
        {
            throw new ArgumentException("birthdate is after today");
        }

        var years = today.Year - birthdate.Year;
        var birthdayNotYetReached =
            today.Month < birthdate.Month ||
            (today.Month == birthdate.Month && today.Day < birthdate.Day);
        return birthdayNotYetReached ? years - 1 : years;
    }
}
