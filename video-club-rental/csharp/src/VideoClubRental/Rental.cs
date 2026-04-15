namespace VideoClubRental;

public class Rental
{
    public const int RentalPeriodDays = 15;

    public Rental(User user, Title title, DateOnly rentedOn)
    {
        User = user;
        Title = title;
        RentedOn = rentedOn;
        DueOn = rentedOn.AddDays(RentalPeriodDays);
    }

    public User User { get; }
    public Title Title { get; }
    public DateOnly RentedOn { get; }
    public DateOnly DueOn { get; }

    public bool IsLateAt(DateOnly returnDate) => returnDate > DueOn;
}
