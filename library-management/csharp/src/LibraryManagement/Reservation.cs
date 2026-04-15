namespace LibraryManagement;

public class Reservation
{
    public const int ReservationExpiryDays = 3;

    public Reservation(Member member, Isbn isbn, DateOnly reservedOn)
    {
        Member = member;
        Isbn = isbn;
        ReservedOn = reservedOn;
    }

    public Member Member { get; }
    public Isbn Isbn { get; }
    public DateOnly ReservedOn { get; }
    public DateOnly? NotifiedOn { get; private set; }

    public bool IsNotified => NotifiedOn is not null;

    public bool HasExpiredAt(DateOnly today) =>
        NotifiedOn is DateOnly n && today.DayNumber - n.DayNumber > ReservationExpiryDays;

    internal void MarkNotified(DateOnly today) => NotifiedOn = today;
}
