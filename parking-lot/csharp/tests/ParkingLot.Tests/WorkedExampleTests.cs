using FluentAssertions;

namespace ParkingLot.Tests;

public class WorkedExampleTests
{
    [Fact]
    public void End_to_end_lot_with_one_of_each_spot_type()
    {
        // 1 motorcycle spot, 1 compact spot, 1 large spot
        var (lot, clock) = new LotBuilder()
            .WithMotorcycleSpots(1).WithCompactSpots(1).WithLargeSpots(1).Build();

        var motorcycle = new VehicleBuilder().AsMotorcycle().WithPlate("MC-001").Build();
        var car = new VehicleBuilder().AsCar().WithPlate("CAR-001").Build();
        var bus = new VehicleBuilder().AsBus().WithPlate("BUS-001").Build();

        // Park all three — fills the lot
        var mcTicket = lot.ParkEntry(motorcycle);
        mcTicket.SpotType.Should().Be(SpotType.Motorcycle);

        var carTicket = lot.ParkEntry(car);
        carTicket.SpotType.Should().Be(SpotType.Compact);

        var busTicket = lot.ParkEntry(bus);
        busTicket.SpotType.Should().Be(SpotType.Large);

        // Exit motorcycle at t+90min → $2 (ceil(1.5) = 2 hours × $1)
        clock.Advance(TimeSpan.FromMinutes(90));
        var mcFee = lot.ProcessExit(mcTicket);
        mcFee.Amount.Should().Be(2m);

        // Try to park a second car — motorcycle spot freed, but car doesn't fit motorcycle spot
        var car2 = new VehicleBuilder().AsCar().WithPlate("CAR-002").Build();
        var action = () => lot.ParkEntry(car2);
        action.Should().Throw<NoAvailableSpotException>()
            .WithMessage("No available spot for vehicle CAR-002");

        // Park a second motorcycle — gets the freed motorcycle spot
        var mc2 = new VehicleBuilder().AsMotorcycle().WithPlate("MC-002").Build();
        var mc2Ticket = lot.ParkEntry(mc2);
        mc2Ticket.SpotType.Should().Be(SpotType.Motorcycle);

        // Exit car at t+30min (total t+120min from start, but car entered at t=0 so 120min = 2h) → $6
        // Actually, clock is now at t+90min. Advance another 30min to t+120min.
        clock.Advance(TimeSpan.FromMinutes(30));
        var carFee = lot.ProcessExit(carTicket);
        carFee.Amount.Should().Be(6m);

        // Exit bus at t+2h from start. Clock is at t+120min = t+2h already. → $10
        var busFee = lot.ProcessExit(busTicket);
        busFee.Amount.Should().Be(10m);
    }
}
