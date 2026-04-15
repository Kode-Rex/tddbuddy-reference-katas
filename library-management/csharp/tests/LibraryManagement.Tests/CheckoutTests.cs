using FluentAssertions;

namespace LibraryManagement.Tests;

public class CheckoutTests
{
    private static readonly DateOnly Day0 = new(2026, 1, 1);
    private static readonly Isbn RefactoringIsbn = new("978-0134757599");

    [Fact]
    public void Checking_out_an_available_copy_marks_the_copy_as_checked_out()
    {
        var member = new MemberBuilder().Build();
        var (library, _, _) = new LibraryBuilder().OpenedOn(Day0).WithMember(member)
            .WithBook(new BookBuilder().WithIsbn(RefactoringIsbn.Value).WithCopies(1)).Build();

        var loan = library.CheckOut(member, RefactoringIsbn);

        loan.Copy.Status.Should().Be(CopyStatus.CheckedOut);
    }

    [Fact]
    public void Checking_out_an_available_copy_creates_a_loan_with_a_due_date_fourteen_days_from_today()
    {
        var member = new MemberBuilder().Build();
        var (library, _, _) = new LibraryBuilder().OpenedOn(Day0).WithMember(member)
            .WithBook(new BookBuilder().WithIsbn(RefactoringIsbn.Value).WithCopies(1)).Build();

        var loan = library.CheckOut(member, RefactoringIsbn);

        loan.BorrowedOn.Should().Be(Day0);
        loan.DueOn.Should().Be(Day0.AddDays(Loan.LoanPeriodDays));
    }

    [Fact]
    public void Checking_out_when_no_copy_is_available_is_rejected()
    {
        var member = new MemberBuilder().Build();
        var other = new MemberBuilder().Named("Other").Build();
        var (library, _, _) = new LibraryBuilder().OpenedOn(Day0)
            .WithMember(member).WithMember(other)
            .WithBook(new BookBuilder().WithIsbn(RefactoringIsbn.Value).WithCopies(1)).Build();
        library.CheckOut(other, RefactoringIsbn);

        var act = () => library.CheckOut(member, RefactoringIsbn);

        act.Should().Throw<InvalidOperationException>();
    }
}
