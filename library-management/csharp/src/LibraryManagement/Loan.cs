namespace LibraryManagement;

public class Loan
{
    public const int LoanPeriodDays = 14;
    public static readonly Money FinePerDay = new(0.10m);

    public Loan(Member member, Copy copy, DateOnly borrowedOn)
    {
        Member = member;
        Copy = copy;
        BorrowedOn = borrowedOn;
        DueOn = borrowedOn.AddDays(LoanPeriodDays);
    }

    public Member Member { get; }
    public Copy Copy { get; }
    public DateOnly BorrowedOn { get; }
    public DateOnly DueOn { get; }
    public DateOnly? ReturnedOn { get; private set; }

    public bool IsClosed => ReturnedOn is not null;

    public Money FineFor(DateOnly returnDate)
    {
        if (returnDate <= DueOn) return Money.Zero;
        var daysLate = returnDate.DayNumber - DueOn.DayNumber;
        return FinePerDay * daysLate;
    }

    internal void Close(DateOnly returnedOn) => ReturnedOn = returnedOn;
}
