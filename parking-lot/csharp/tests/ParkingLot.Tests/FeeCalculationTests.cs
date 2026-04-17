using FluentAssertions;

namespace ParkingLot.Tests;

public class FeeCalculationTests
{
    [Fact]
    public void A_motorcycle_parked_for_exactly_one_hour_pays_1_dollar()
    {
        var (lot, clock) = new LotBuilder()
            .WithMotorcycleSpots(1).WithCompactSpots(0).WithLargeSpots(0).Build();
        var motorcycle = new VehicleBuilder().AsMotorcycle().Build();

        var ticket = lot.ParkEntry(motorcycle);
        clock.Advance(TimeSpan.FromHours(1));
        var fee = lot.ProcessExit(ticket);

        fee.Amount.Should().Be(1m);
    }

    [Fact]
    public void A_car_parked_for_exactly_two_hours_pays_6_dollars()
    {
        var (lot, clock) = new LotBuilder()
            .WithMotorcycleSpots(0).WithCompactSpots(1).WithLargeSpots(0).Build();
        var car = new VehicleBuilder().AsCar().Build();

        var ticket = lot.ParkEntry(car);
        clock.Advance(TimeSpan.FromHours(2));
        var fee = lot.ProcessExit(ticket);

        fee.Amount.Should().Be(6m);
    }

    [Fact]
    public void A_bus_parked_for_exactly_one_hour_pays_5_dollars()
    {
        var (lot, clock) = new LotBuilder()
            .WithMotorcycleSpots(0).WithCompactSpots(0).WithLargeSpots(1).Build();
        var bus = new VehicleBuilder().AsBus().Build();

        var ticket = lot.ParkEntry(bus);
        clock.Advance(TimeSpan.FromHours(1));
        var fee = lot.ProcessExit(ticket);

        fee.Amount.Should().Be(5m);
    }

    [Fact]
    public void Partial_hours_round_up_a_car_parked_for_2_hours_1_minute_pays_9_dollars()
    {
        var (lot, clock) = new LotBuilder()
            .WithMotorcycleSpots(0).WithCompactSpots(1).WithLargeSpots(0).Build();
        var car = new VehicleBuilder().AsCar().Build();

        var ticket = lot.ParkEntry(car);
        clock.Advance(TimeSpan.FromHours(2).Add(TimeSpan.FromMinutes(1)));
        var fee = lot.ProcessExit(ticket);

        fee.Amount.Should().Be(9m);
    }

    [Fact]
    public void A_stay_of_zero_duration_is_billed_as_1_hour()
    {
        var (lot, _) = new LotBuilder()
            .WithMotorcycleSpots(0).WithCompactSpots(1).WithLargeSpots(0).Build();
        var car = new VehicleBuilder().AsCar().Build();

        var ticket = lot.ParkEntry(car);
        // No clock advance — exit at the same time as entry
        var fee = lot.ProcessExit(ticket);

        fee.Amount.Should().Be(3m);
    }
}
