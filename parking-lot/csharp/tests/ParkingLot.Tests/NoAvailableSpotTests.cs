using FluentAssertions;

namespace ParkingLot.Tests;

public class NoAvailableSpotTests
{
    [Fact]
    public void Parking_a_car_when_only_motorcycle_spots_remain_raises_NoAvailableSpotException()
    {
        var (lot, _) = new LotBuilder()
            .WithMotorcycleSpots(2).WithCompactSpots(0).WithLargeSpots(0).Build();
        var car = new VehicleBuilder().AsCar().WithPlate("CAR-100").Build();

        var action = () => lot.ParkEntry(car);

        action.Should().Throw<NoAvailableSpotException>()
            .WithMessage("No available spot for vehicle CAR-100");
    }

    [Fact]
    public void Parking_a_bus_when_only_compact_and_motorcycle_spots_remain_raises_NoAvailableSpotException()
    {
        var (lot, _) = new LotBuilder()
            .WithMotorcycleSpots(1).WithCompactSpots(2).WithLargeSpots(0).Build();
        var bus = new VehicleBuilder().AsBus().WithPlate("BUS-100").Build();

        var action = () => lot.ParkEntry(bus);

        action.Should().Throw<NoAvailableSpotException>()
            .WithMessage("No available spot for vehicle BUS-100");
    }

    [Fact]
    public void Parking_any_vehicle_when_the_lot_is_completely_full_raises_NoAvailableSpotException()
    {
        var (lot, _) = new LotBuilder()
            .WithMotorcycleSpots(1).WithCompactSpots(0).WithLargeSpots(0).Build();
        lot.ParkEntry(new VehicleBuilder().AsMotorcycle().WithPlate("MC-001").Build());

        var action = () => lot.ParkEntry(new VehicleBuilder().AsMotorcycle().WithPlate("MC-002").Build());

        action.Should().Throw<NoAvailableSpotException>()
            .WithMessage("No available spot for vehicle MC-002");
    }
}
