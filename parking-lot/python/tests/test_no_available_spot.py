import pytest

from parking_lot import NoAvailableSpotError
from tests.lot_builder import LotBuilder
from tests.vehicle_builder import VehicleBuilder


class TestNoAvailableSpot:
    def test_parking_a_car_when_only_motorcycle_spots_remain_raises_no_available_spot_error(self) -> None:
        lot, _ = LotBuilder().with_motorcycle_spots(2).with_compact_spots(0).with_large_spots(0).build()
        car = VehicleBuilder().as_car().with_plate("CAR-100").build()

        with pytest.raises(NoAvailableSpotError, match="No available spot for vehicle CAR-100"):
            lot.park_entry(car)

    def test_parking_a_bus_when_only_compact_and_motorcycle_spots_remain_raises_no_available_spot_error(self) -> None:
        lot, _ = LotBuilder().with_motorcycle_spots(1).with_compact_spots(2).with_large_spots(0).build()
        bus = VehicleBuilder().as_bus().with_plate("BUS-100").build()

        with pytest.raises(NoAvailableSpotError, match="No available spot for vehicle BUS-100"):
            lot.park_entry(bus)

    def test_parking_any_vehicle_when_the_lot_is_completely_full_raises_no_available_spot_error(self) -> None:
        lot, _ = LotBuilder().with_motorcycle_spots(1).with_compact_spots(0).with_large_spots(0).build()
        lot.park_entry(VehicleBuilder().as_motorcycle().with_plate("MC-001").build())

        with pytest.raises(NoAvailableSpotError, match="No available spot for vehicle MC-002"):
            lot.park_entry(VehicleBuilder().as_motorcycle().with_plate("MC-002").build())
