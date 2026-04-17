using FluentAssertions;

namespace ParkingLot.Tests;

public class LotConstructionTests
{
    [Fact]
    public void A_lot_with_spots_across_all_types_is_valid()
    {
        var action = () => new LotBuilder()
            .WithMotorcycleSpots(2)
            .WithCompactSpots(3)
            .WithLargeSpots(1)
            .Build();

        action.Should().NotThrow();
    }

    [Fact]
    public void A_lot_with_zero_total_spots_raises_InvalidLotConfigurationException()
    {
        var action = () => new LotBuilder()
            .WithMotorcycleSpots(0)
            .WithCompactSpots(0)
            .WithLargeSpots(0)
            .Build();

        action.Should().Throw<InvalidLotConfigurationException>()
            .WithMessage("Lot must have at least one spot");
    }
}
