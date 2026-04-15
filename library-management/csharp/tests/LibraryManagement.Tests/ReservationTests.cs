using FluentAssertions;

namespace LibraryManagement.Tests;

public class ReservationTests
{
    private static readonly DateOnly Day0 = new(2026, 1, 1);
    private static readonly Isbn RefactoringIsbn = new("978-0134757599");

    [Fact]
    public void Reserving_an_unavailable_book_adds_the_member_to_the_queue()
    {
        var borrower = new MemberBuilder().Named("Borrower").Build();
        var reserver = new MemberBuilder().Named("Reserver").Build();
        var (library, _, _) = new LibraryBuilder().OpenedOn(Day0)
            .WithMember(borrower).WithMember(reserver)
            .WithBook(new BookBuilder().WithIsbn(RefactoringIsbn.Value).WithCopies(1)).Build();
        library.CheckOut(borrower, RefactoringIsbn);

        library.Reserve(reserver, RefactoringIsbn);

        library.ReservationsFor(RefactoringIsbn).Should().ContainSingle()
            .Which.Member.Should().BeSameAs(reserver);
    }

    [Fact]
    public void Returning_a_copy_with_a_non_empty_queue_marks_the_copy_as_reserved()
    {
        var borrower = new MemberBuilder().Named("Borrower").Build();
        var reserver = new MemberBuilder().Named("Reserver").Build();
        var (library, _, clock) = new LibraryBuilder().OpenedOn(Day0)
            .WithMember(borrower).WithMember(reserver)
            .WithBook(new BookBuilder().WithIsbn(RefactoringIsbn.Value).WithCopies(1)).Build();
        library.CheckOut(borrower, RefactoringIsbn);
        library.Reserve(reserver, RefactoringIsbn);
        clock.AdvanceDays(2);

        library.ReturnCopy(borrower, RefactoringIsbn);

        library.Books.Single().Copies.Single().Status.Should().Be(CopyStatus.Reserved);
    }

    [Fact]
    public void Returning_a_copy_with_a_non_empty_queue_notifies_the_head_of_the_queue()
    {
        var borrower = new MemberBuilder().Named("Borrower").Build();
        var reserver = new MemberBuilder().Named("Reserver").Build();
        var (library, notifier, clock) = new LibraryBuilder().OpenedOn(Day0)
            .WithMember(borrower).WithMember(reserver)
            .WithBook(new BookBuilder().Titled("Refactoring").WithIsbn(RefactoringIsbn.Value).WithCopies(1)).Build();
        library.CheckOut(borrower, RefactoringIsbn);
        library.Reserve(reserver, RefactoringIsbn);
        clock.AdvanceDays(2);

        library.ReturnCopy(borrower, RefactoringIsbn);

        notifier.AvailabilityNotificationsFor(reserver).Should().ContainSingle()
            .Which.Message.Should().Contain("Refactoring");
    }

    [Fact]
    public void Reservations_older_than_three_days_expire_and_the_next_reserver_is_notified()
    {
        var borrower = new MemberBuilder().Named("Borrower").Build();
        var first = new MemberBuilder().Named("First").Build();
        var second = new MemberBuilder().Named("Second").Build();
        var (library, notifier, clock) = new LibraryBuilder().OpenedOn(Day0)
            .WithMember(borrower).WithMember(first).WithMember(second)
            .WithBook(new BookBuilder().Titled("Refactoring").WithIsbn(RefactoringIsbn.Value).WithCopies(1)).Build();
        library.CheckOut(borrower, RefactoringIsbn);
        library.Reserve(first, RefactoringIsbn);
        library.Reserve(second, RefactoringIsbn);
        library.ReturnCopy(borrower, RefactoringIsbn); // first is notified here

        clock.AdvanceDays(Reservation.ReservationExpiryDays + 1);
        library.ExpireReservations();

        notifier.ExpirationNotificationsFor(first).Should().ContainSingle();
        notifier.AvailabilityNotificationsFor(second).Should().ContainSingle();
    }

    [Fact]
    public void Checking_out_a_reserved_copy_satisfies_the_reservation_and_clears_it()
    {
        var borrower = new MemberBuilder().Named("Borrower").Build();
        var reserver = new MemberBuilder().Named("Reserver").Build();
        var (library, _, clock) = new LibraryBuilder().OpenedOn(Day0)
            .WithMember(borrower).WithMember(reserver)
            .WithBook(new BookBuilder().WithIsbn(RefactoringIsbn.Value).WithCopies(1)).Build();
        library.CheckOut(borrower, RefactoringIsbn);
        library.Reserve(reserver, RefactoringIsbn);
        library.ReturnCopy(borrower, RefactoringIsbn); // copy is now Reserved
        clock.AdvanceDays(1);

        var loan = library.CheckOut(reserver, RefactoringIsbn);

        loan.Copy.Status.Should().Be(CopyStatus.CheckedOut);
        library.ReservationsFor(RefactoringIsbn).Should().BeEmpty();
    }
}
