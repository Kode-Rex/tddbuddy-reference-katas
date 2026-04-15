using FluentAssertions;
using Xunit;

namespace VideoClubRental.Tests;

public class DonationTests
{
    [Fact]
    public void Donating_a_new_title_creates_a_library_entry_with_one_copy()
    {
        var donor = new UserBuilder().Build();
        var (club, _, _) = new VideoClubBuilder().WithUser(donor).Build();

        club.Donate(donor, "Rushmore");

        club.Titles.Should().ContainSingle(t => t.Name == "Rushmore")
            .Which.TotalCopies.Should().Be(1);
    }

    [Fact]
    public void Donating_an_existing_title_increments_its_copy_count()
    {
        var donor = new UserBuilder().Build();
        var (club, _, _) = new VideoClubBuilder()
            .WithUser(donor)
            .WithTitle(new TitleBuilder().Named("Jaws").WithCopies(2).Build())
            .Build();

        club.Donate(donor, "Jaws");

        var jaws = club.Titles.Single(t => t.Name == "Jaws");
        jaws.TotalCopies.Should().Be(3);
        jaws.AvailableCopies.Should().Be(3);
    }

    [Fact]
    public void Donating_awards_ten_loyalty_points_to_the_donor()
    {
        var donor = new UserBuilder().Build();
        var (club, _, _) = new VideoClubBuilder().WithUser(donor).Build();

        club.Donate(donor, "Rushmore");

        donor.LoyaltyPoints.Should().Be(10);
    }
}
