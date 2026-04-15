using FluentAssertions;
using Xunit;

namespace VideoClubRental.Tests;

public class PriorityAccessTests
{
    [Fact]
    public void User_with_five_priority_points_has_priority_access_to_new_releases()
    {
        var user = new UserBuilder().WithPriorityPoints(5).Build();
        var (club, _, _) = new VideoClubBuilder().WithUser(user).Build();

        club.HasPriorityAccess(user).Should().BeTrue();
    }

    [Fact]
    public void User_with_four_priority_points_does_not_have_priority_access()
    {
        var user = new UserBuilder().WithPriorityPoints(4).Build();
        var (club, _, _) = new VideoClubBuilder().WithUser(user).Build();

        club.HasPriorityAccess(user).Should().BeFalse();
    }

    [Fact]
    public void Priority_points_cannot_go_below_zero()
    {
        var user = new UserBuilder().WithPriorityPoints(1).Build();
        var (club, _, clock) = new VideoClubBuilder()
            .WithUser(user)
            .WithTitle(new TitleBuilder().Named("Jaws").Build()).Build();
        club.Rent(user, "Jaws");
        clock.AdvanceDays(Rental.RentalPeriodDays + 1);
        club.ReturnTitle(user, "Jaws");

        user.PriorityPoints.Should().Be(0);
    }
}
