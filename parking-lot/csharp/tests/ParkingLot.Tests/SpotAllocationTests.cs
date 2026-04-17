using FluentAssertions;

namespace ParkingLot.Tests;

public class SpotAllocationTests
{
    [Fact]
    public void A_motorcycle_parks_in_a_motorcycle_spot_when_available()
    {
        var (lot, _) = new LotBuilder()
            .WithMotorcycleSpots(1).WithCompactSpots(1).WithLargeSpots(1).Build();
        var motorcycle = new VehicleBuilder().AsMotorcycle().Build();

        var ticket = lot.ParkEntry(motorcycle);

        ticket.SpotType.Should().Be(SpotType.Motorcycle);
    }

    [Fact]
    public void A_car_parks_in_a_compact_spot_when_available()
    {
        var (lot, _) = new LotBuilder()
            .WithMotorcycleSpots(1).WithCompactSpots(1).WithLargeSpots(1).Build();
        var car = new VehicleBuilder().AsCar().Build();

        var ticket = lot.ParkEntry(car);

        ticket.SpotType.Should().Be(SpotType.Compact);
    }

    [Fact]
    public void A_bus_parks_in_a_large_spot_when_available()
    {
        var (lot, _) = new LotBuilder()
            .WithMotorcycleSpots(1).WithCompactSpots(1).WithLargeSpots(1).Build();
        var bus = new VehicleBuilder().AsBus().Build();

        var ticket = lot.ParkEntry(bus);

        ticket.SpotType.Should().Be(SpotType.Large);
    }

    [Fact]
    public void A_motorcycle_uses_a_compact_spot_when_no_motorcycle_spots_remain()
    {
        var (lot, _) = new LotBuilder()
            .WithMotorcycleSpots(0).WithCompactSpots(1).WithLargeSpots(1).Build();
        var motorcycle = new VehicleBuilder().AsMotorcycle().Build();

        var ticket = lot.ParkEntry(motorcycle);

        ticket.SpotType.Should().Be(SpotType.Compact);
    }

    [Fact]
    public void A_motorcycle_uses_a_large_spot_when_no_motorcycle_or_compact_spots_remain()
    {
        var (lot, _) = new LotBuilder()
            .WithMotorcycleSpots(0).WithCompactSpots(0).WithLargeSpots(1).Build();
        var motorcycle = new VehicleBuilder().AsMotorcycle().Build();

        var ticket = lot.ParkEntry(motorcycle);

        ticket.SpotType.Should().Be(SpotType.Large);
    }

    [Fact]
    public void A_car_uses_a_large_spot_when_no_compact_spots_remain()
    {
        var (lot, _) = new LotBuilder()
            .WithMotorcycleSpots(1).WithCompactSpots(0).WithLargeSpots(1).Build();
        var car = new VehicleBuilder().AsCar().Build();

        var ticket = lot.ParkEntry(car);

        ticket.SpotType.Should().Be(SpotType.Large);
    }
}
