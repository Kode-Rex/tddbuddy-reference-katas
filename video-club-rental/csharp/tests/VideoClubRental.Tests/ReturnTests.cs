using FluentAssertions;
using Xunit;

namespace VideoClubRental.Tests;

public class ReturnTests
{
    private static readonly DateOnly Day0 = new(2026, 1, 1);

    [Fact]
    public void On_time_return_awards_two_priority_points()
    {
        var user = new UserBuilder().Build();
        var (club, _, clock) = new VideoClubBuilder()
            .OpenedOn(Day0).WithUser(user)
            .WithTitle(new TitleBuilder().Named("Jaws").Build()).Build();
        club.Rent(user, "Jaws");

        clock.AdvanceDays(Rental.RentalPeriodDays);
        club.ReturnTitle(user, "Jaws");

        user.PriorityPoints.Should().Be(2);
    }

    [Fact]
    public void Late_return_deducts_two_priority_points()
    {
        var user = new UserBuilder().WithPriorityPoints(4).Build();
        var (club, _, clock) = new VideoClubBuilder()
            .OpenedOn(Day0).WithUser(user)
            .WithTitle(new TitleBuilder().Named("Jaws").Build()).Build();
        club.Rent(user, "Jaws");

        clock.AdvanceDays(Rental.RentalPeriodDays + 1);
        club.ReturnTitle(user, "Jaws");

        user.PriorityPoints.Should().Be(2);
    }

    [Fact]
    public void Late_return_dispatches_a_late_alert()
    {
        var user = new UserBuilder().Build();
        var (club, notifier, clock) = new VideoClubBuilder()
            .OpenedOn(Day0).WithUser(user)
            .WithTitle(new TitleBuilder().Named("Jaws").Build()).Build();
        club.Rent(user, "Jaws");

        clock.AdvanceDays(Rental.RentalPeriodDays + 3);
        club.ReturnTitle(user, "Jaws");

        notifier.NotificationsFor(user).Should().ContainSingle(n => n.Message.Contains("overdue"));
    }

    [Fact]
    public void User_with_an_overdue_rental_cannot_rent_another_title()
    {
        var user = new UserBuilder().Build();
        var (club, _, clock) = new VideoClubBuilder()
            .OpenedOn(Day0).WithUser(user)
            .WithTitle(new TitleBuilder().Named("Jaws").Build())
            .WithTitle(new TitleBuilder().Named("Casablanca").Build()).Build();
        club.Rent(user, "Jaws");

        clock.AdvanceDays(Rental.RentalPeriodDays + 1);
        club.MarkOverdueRentals();

        var act = () => club.Rent(user, "Casablanca");
        act.Should().Throw<OverdueRentalException>();
    }

    [Fact]
    public void Returning_the_overdue_title_unblocks_renting()
    {
        var user = new UserBuilder().Build();
        var (club, _, clock) = new VideoClubBuilder()
            .OpenedOn(Day0).WithUser(user)
            .WithTitle(new TitleBuilder().Named("Jaws").Build())
            .WithTitle(new TitleBuilder().Named("Casablanca").Build()).Build();
        club.Rent(user, "Jaws");

        clock.AdvanceDays(Rental.RentalPeriodDays + 1);
        club.MarkOverdueRentals();
        club.ReturnTitle(user, "Jaws");

        var cost = club.Rent(user, "Casablanca");
        cost.Should().Be(new Money(2.50m));
    }
}
