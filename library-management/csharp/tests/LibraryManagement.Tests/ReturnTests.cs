using FluentAssertions;

namespace LibraryManagement.Tests;

public class ReturnTests
{
    private static readonly DateOnly Day0 = new(2026, 1, 1);
    private static readonly Isbn RefactoringIsbn = new("978-0134757599");

    private static (Library, RecordingNotifier, FixedClock, Member) OpenLibraryWithActiveLoan()
    {
        var member = new MemberBuilder().Build();
        var (library, notifier, clock) = new LibraryBuilder().OpenedOn(Day0).WithMember(member)
            .WithBook(new BookBuilder().WithIsbn(RefactoringIsbn.Value).WithCopies(1)).Build();
        library.CheckOut(member, RefactoringIsbn);
        return (library, notifier, clock, member);
    }

    [Fact]
    public void Returning_a_checked_out_copy_marks_the_copy_as_available()
    {
        var (library, _, clock, member) = OpenLibraryWithActiveLoan();
        clock.AdvanceDays(5);

        library.ReturnCopy(member, RefactoringIsbn);

        library.Books.Single().Copies.Single().Status.Should().Be(CopyStatus.Available);
    }

    [Fact]
    public void Returning_a_copy_closes_the_loan()
    {
        var (library, _, clock, member) = OpenLibraryWithActiveLoan();
        clock.AdvanceDays(5);

        library.ReturnCopy(member, RefactoringIsbn);

        library.Loans.Should().BeEmpty();
    }

    [Fact]
    public void Returning_on_time_incurs_no_fine()
    {
        var (library, _, clock, member) = OpenLibraryWithActiveLoan();
        clock.AdvanceDays(Loan.LoanPeriodDays);

        var fine = library.ReturnCopy(member, RefactoringIsbn);

        fine.Should().Be(Money.Zero);
    }

    [Fact]
    public void Returning_one_day_late_incurs_a_ten_pence_fine()
    {
        var (library, _, clock, member) = OpenLibraryWithActiveLoan();
        clock.AdvanceDays(Loan.LoanPeriodDays + 1);

        var fine = library.ReturnCopy(member, RefactoringIsbn);

        fine.Should().Be(new Money(0.10m));
    }

    [Fact]
    public void Returning_ten_days_late_incurs_a_one_pound_fine()
    {
        var (library, _, clock, member) = OpenLibraryWithActiveLoan();
        clock.AdvanceDays(Loan.LoanPeriodDays + 10);

        var fine = library.ReturnCopy(member, RefactoringIsbn);

        fine.Should().Be(new Money(1.00m));
    }
}
