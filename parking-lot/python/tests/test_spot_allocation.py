from parking_lot import SpotType
from tests.lot_builder import LotBuilder
from tests.vehicle_builder import VehicleBuilder


class TestSpotAllocation:
    def test_a_motorcycle_parks_in_a_motorcycle_spot_when_available(self) -> None:
        lot, _ = LotBuilder().with_motorcycle_spots(1).with_compact_spots(1).with_large_spots(1).build()
        motorcycle = VehicleBuilder().as_motorcycle().build()

        ticket = lot.park_entry(motorcycle)

        assert ticket.spot_type == SpotType.MOTORCYCLE

    def test_a_car_parks_in_a_compact_spot_when_available(self) -> None:
        lot, _ = LotBuilder().with_motorcycle_spots(1).with_compact_spots(1).with_large_spots(1).build()
        car = VehicleBuilder().as_car().build()

        ticket = lot.park_entry(car)

        assert ticket.spot_type == SpotType.COMPACT

    def test_a_bus_parks_in_a_large_spot_when_available(self) -> None:
        lot, _ = LotBuilder().with_motorcycle_spots(1).with_compact_spots(1).with_large_spots(1).build()
        bus = VehicleBuilder().as_bus().build()

        ticket = lot.park_entry(bus)

        assert ticket.spot_type == SpotType.LARGE

    def test_a_motorcycle_uses_a_compact_spot_when_no_motorcycle_spots_remain(self) -> None:
        lot, _ = LotBuilder().with_motorcycle_spots(0).with_compact_spots(1).with_large_spots(1).build()
        motorcycle = VehicleBuilder().as_motorcycle().build()

        ticket = lot.park_entry(motorcycle)

        assert ticket.spot_type == SpotType.COMPACT

    def test_a_motorcycle_uses_a_large_spot_when_no_motorcycle_or_compact_spots_remain(self) -> None:
        lot, _ = LotBuilder().with_motorcycle_spots(0).with_compact_spots(0).with_large_spots(1).build()
        motorcycle = VehicleBuilder().as_motorcycle().build()

        ticket = lot.park_entry(motorcycle)

        assert ticket.spot_type == SpotType.LARGE

    def test_a_car_uses_a_large_spot_when_no_compact_spots_remain(self) -> None:
        lot, _ = LotBuilder().with_motorcycle_spots(1).with_compact_spots(0).with_large_spots(1).build()
        car = VehicleBuilder().as_car().build()

        ticket = lot.park_entry(car)

        assert ticket.spot_type == SpotType.LARGE
