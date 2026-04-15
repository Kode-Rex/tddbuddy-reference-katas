using FluentAssertions;
using Xunit;

namespace VideoClubRental.Tests;

public class RentalPricingTests
{
    private static (VideoClub Club, User User) ClubWithStockedCatalog()
    {
        var user = new UserBuilder().Build();
        var (club, _, _) = new VideoClubBuilder()
            .WithUser(user)
            .WithTitle(new TitleBuilder().Named("The Godfather").Build())
            .WithTitle(new TitleBuilder().Named("Casablanca").Build())
            .WithTitle(new TitleBuilder().Named("Jaws").Build())
            .Build();
        return (club, user);
    }

    [Fact]
    public void First_simultaneous_rental_costs_two_pounds_fifty()
    {
        var (club, user) = ClubWithStockedCatalog();

        var cost = club.Rent(user, "The Godfather");

        cost.Should().Be(new Money(2.50m));
    }

    [Fact]
    public void Second_simultaneous_rental_costs_two_pounds_twenty_five()
    {
        var (club, user) = ClubWithStockedCatalog();
        club.Rent(user, "The Godfather");

        var cost = club.Rent(user, "Casablanca");

        cost.Should().Be(new Money(2.25m));
    }

    [Fact]
    public void Third_simultaneous_rental_costs_one_pound_seventy_five()
    {
        var (club, user) = ClubWithStockedCatalog();
        club.Rent(user, "The Godfather");
        club.Rent(user, "Casablanca");

        var cost = club.Rent(user, "Jaws");

        cost.Should().Be(new Money(1.75m));
    }

    [Fact]
    public void Renting_two_titles_charges_four_pounds_seventy_five_total()
    {
        var (club, user) = ClubWithStockedCatalog();

        var first = club.Rent(user, "The Godfather");
        var second = club.Rent(user, "Casablanca");

        (first + second).Should().Be(new Money(4.75m));
    }

    [Fact]
    public void Renting_three_titles_charges_six_pounds_fifty_total()
    {
        var (club, user) = ClubWithStockedCatalog();

        var first = club.Rent(user, "The Godfather");
        var second = club.Rent(user, "Casablanca");
        var third = club.Rent(user, "Jaws");

        (first + second + third).Should().Be(new Money(6.50m));
    }
}
