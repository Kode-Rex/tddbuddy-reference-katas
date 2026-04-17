using FluentAssertions;

namespace ParkingLot.Tests;

public class ExitTests
{
    [Fact]
    public void Exiting_frees_the_spot_so_another_vehicle_of_the_same_type_can_park()
    {
        var (lot, clock) = new LotBuilder()
            .WithMotorcycleSpots(0).WithCompactSpots(1).WithLargeSpots(0).Build();
        var car1 = new VehicleBuilder().AsCar().WithPlate("CAR-001").Build();
        var car2 = new VehicleBuilder().AsCar().WithPlate("CAR-002").Build();

        var ticket = lot.ParkEntry(car1);
        clock.Advance(TimeSpan.FromHours(1));
        lot.ProcessExit(ticket);

        var action = () => lot.ParkEntry(car2);
        action.Should().NotThrow();
    }

    [Fact]
    public void Exiting_with_an_invalid_ticket_raises_InvalidTicketException()
    {
        var (lot, _) = new LotBuilder()
            .WithMotorcycleSpots(1).WithCompactSpots(1).WithLargeSpots(1).Build();
        var fakeTicket = new Ticket(
            new Vehicle(VehicleType.Car, "FAKE-001"),
            SpotType.Compact,
            new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc));

        var action = () => lot.ProcessExit(fakeTicket);

        action.Should().Throw<InvalidTicketException>()
            .WithMessage("Ticket is not valid");
    }

    [Fact]
    public void Exiting_with_the_same_ticket_twice_raises_InvalidTicketException()
    {
        var (lot, clock) = new LotBuilder()
            .WithMotorcycleSpots(1).WithCompactSpots(1).WithLargeSpots(1).Build();
        var car = new VehicleBuilder().AsCar().Build();
        var ticket = lot.ParkEntry(car);

        clock.Advance(TimeSpan.FromHours(1));
        lot.ProcessExit(ticket);

        var action = () => lot.ProcessExit(ticket);
        action.Should().Throw<InvalidTicketException>()
            .WithMessage("Ticket is not valid");
    }
}
