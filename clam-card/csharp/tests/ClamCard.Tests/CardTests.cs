using FluentAssertions;
using Xunit;

namespace ClamCard.Tests;

public class CardTests
{
    // A card on a network with both zones populated from the kata spec's
    // station table: Zone A = Asterisk, Amersham, Aldgate, Angel, Anerley
    // (plus Antelope for the four-ride Zone A cap scenario); Zone B =
    // Bison, Bugel, Balham, Bullhead, Barbican.
    private static CardBuilder AMichaelCard() =>
        new CardBuilder()
            .OnDay(new DateOnly(2024, 1, 1))
            .WithZone(Zone.A, "Asterisk", "Amersham", "Aldgate", "Angel", "Anerley", "Antelope")
            .WithZone(Zone.B, "Bison", "Bugel", "Balham", "Bullhead", "Barbican");

    [Fact]
    public void One_way_Zone_A_journey_is_charged_the_Zone_A_single_fare()
    {
        var card = AMichaelCard().Build();

        var ride = card.TravelFrom("Asterisk").To("Aldgate");

        ride.Fare.Should().Be(2.50m);
        ride.Zone.Should().Be(Zone.A);
    }

    [Fact]
    public void One_way_Zone_A_to_B_journey_is_charged_the_Zone_B_single_fare()
    {
        var card = AMichaelCard().Build();

        var ride = card.TravelFrom("Asterisk").To("Barbican");

        ride.Fare.Should().Be(3.00m);
        ride.Zone.Should().Be(Zone.B);
    }

    [Fact]
    public void One_way_Zone_B_to_A_journey_is_charged_the_Zone_B_single_fare()
    {
        var card = AMichaelCard().Build();

        var ride = card.TravelFrom("Bison").To("Asterisk");

        ride.Fare.Should().Be(3.00m);
        ride.Zone.Should().Be(Zone.B);
    }

    [Fact]
    public void One_way_Zone_B_journey_is_charged_the_Zone_B_single_fare()
    {
        var card = AMichaelCard().Build();

        var ride = card.TravelFrom("Bison").To("Barbican");

        ride.Fare.Should().Be(3.00m);
        ride.Zone.Should().Be(Zone.B);
    }

    [Fact]
    public void Two_single_journeys_accumulate_on_total_charged()
    {
        var card = AMichaelCard().Build();

        card.TravelFrom("Asterisk").To("Aldgate");   // Zone A, $2.50
        card.TravelFrom("Asterisk").To("Balham");    // Zone B, $3.00

        card.TotalCharged().Should().Be(5.50m);
        card.Rides().Should().HaveCount(2);
        card.Rides()[0].Fare.Should().Be(2.50m);
        card.Rides()[1].Fare.Should().Be(3.00m);
    }

    [Fact]
    public void Zone_A_daily_cap_is_7_dollars()
    {
        var card = AMichaelCard().Build();

        var r1 = card.TravelFrom("Asterisk").To("Aldgate");
        var r2 = card.TravelFrom("Aldgate").To("Angel");
        var r3 = card.TravelFrom("Angel").To("Antelope");
        var r4 = card.TravelFrom("Antelope").To("Asterisk");

        r1.Fare.Should().Be(2.50m);
        r2.Fare.Should().Be(2.50m);
        r3.Fare.Should().Be(2.00m);
        r4.Fare.Should().Be(0.00m);
        card.TotalCharged().Should().Be(7.00m);
    }

    [Fact]
    public void Zone_B_daily_cap_is_8_dollars()
    {
        var card = AMichaelCard().Build();

        var r1 = card.TravelFrom("Asterisk").To("Barbican");
        var r2 = card.TravelFrom("Barbican").To("Balham");
        var r3 = card.TravelFrom("Balham").To("Bison");
        var r4 = card.TravelFrom("Bison").To("Asterisk");

        r1.Fare.Should().Be(3.00m);
        r2.Fare.Should().Be(3.00m);
        r3.Fare.Should().Be(2.00m);
        r4.Fare.Should().Be(0.00m);
        card.TotalCharged().Should().Be(8.00m);
    }

    [Fact]
    public void Reaching_the_Zone_A_cap_does_not_affect_Zone_B_fares()
    {
        var card = AMichaelCard().Build();

        // Burn the Zone A cap.
        card.TravelFrom("Asterisk").To("Aldgate");
        card.TravelFrom("Aldgate").To("Angel");
        card.TravelFrom("Angel").To("Antelope");
        card.TravelFrom("Antelope").To("Asterisk");

        var nextB = card.TravelFrom("Asterisk").To("Barbican");

        nextB.Fare.Should().Be(3.00m);
    }

    [Fact]
    public void Reaching_the_Zone_B_cap_does_not_affect_Zone_A_fares()
    {
        var card = AMichaelCard().Build();

        // Burn the Zone B cap.
        card.TravelFrom("Asterisk").To("Barbican");
        card.TravelFrom("Barbican").To("Balham");
        card.TravelFrom("Balham").To("Bison");
        card.TravelFrom("Bison").To("Asterisk");

        var nextA = card.TravelFrom("Asterisk").To("Aldgate");

        nextA.Fare.Should().Be(2.50m);
    }

    [Fact]
    public void Travelling_from_an_unknown_station_raises()
    {
        var card = AMichaelCard().Build();

        var act = () => card.TravelFrom("Moonbase");

        act.Should().Throw<UnknownStationException>()
            .WithMessage("station is not on this card's network");
    }

    [Fact]
    public void Travelling_to_an_unknown_station_raises()
    {
        var card = AMichaelCard().Build();

        var act = () => card.TravelFrom("Asterisk").To("Moonbase");

        act.Should().Throw<UnknownStationException>()
            .WithMessage("station is not on this card's network");
    }

    [Fact]
    public void Each_ride_records_its_zone_and_fare()
    {
        var card = AMichaelCard().Build();

        card.TravelFrom("Asterisk").To("Barbican");
        card.TravelFrom("Asterisk").To("Aldgate");

        var expectedB = new RideBuilder()
            .From("Asterisk").To("Barbican").ChargedAt(Zone.B).WithFare(3.00m).Build();
        var expectedA = new RideBuilder()
            .From("Asterisk").To("Aldgate").ChargedAt(Zone.A).WithFare(2.50m).Build();

        card.Rides().Should().ContainInOrder(expectedB, expectedA);
    }
}
