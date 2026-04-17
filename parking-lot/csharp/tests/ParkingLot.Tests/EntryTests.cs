using FluentAssertions;

namespace ParkingLot.Tests;

public class EntryTests
{
    [Fact]
    public void Parking_a_vehicle_returns_a_ticket_with_the_vehicle_and_assigned_spot_type()
    {
        var (lot, _) = new LotBuilder()
            .WithMotorcycleSpots(1).WithCompactSpots(1).WithLargeSpots(1).Build();
        var car = new VehicleBuilder().AsCar().WithPlate("CAR-100").Build();

        var ticket = lot.ParkEntry(car);

        ticket.Vehicle.Should().Be(car);
        ticket.SpotType.Should().Be(SpotType.Compact);
    }

    [Fact]
    public void Parking_the_same_vehicle_twice_raises_VehicleAlreadyParkedException()
    {
        var (lot, _) = new LotBuilder()
            .WithMotorcycleSpots(1).WithCompactSpots(2).WithLargeSpots(1).Build();
        var car = new VehicleBuilder().AsCar().WithPlate("CAR-100").Build();
        lot.ParkEntry(car);

        var action = () => lot.ParkEntry(car);

        action.Should().Throw<VehicleAlreadyParkedException>()
            .WithMessage("Vehicle CAR-100 is already parked");
    }
}
