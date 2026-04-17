import pytest

from parking_lot import SpotType, VehicleAlreadyParkedError
from tests.lot_builder import LotBuilder
from tests.vehicle_builder import VehicleBuilder


class TestEntry:
    def test_parking_a_vehicle_returns_a_ticket_with_the_vehicle_and_assigned_spot_type(self) -> None:
        lot, _ = LotBuilder().with_motorcycle_spots(1).with_compact_spots(1).with_large_spots(1).build()
        car = VehicleBuilder().as_car().with_plate("CAR-100").build()

        ticket = lot.park_entry(car)

        assert ticket.vehicle == car
        assert ticket.spot_type == SpotType.COMPACT

    def test_parking_the_same_vehicle_twice_raises_vehicle_already_parked_error(self) -> None:
        lot, _ = LotBuilder().with_motorcycle_spots(1).with_compact_spots(2).with_large_spots(1).build()
        car = VehicleBuilder().as_car().with_plate("CAR-100").build()
        lot.park_entry(car)

        with pytest.raises(VehicleAlreadyParkedError, match="Vehicle CAR-100 is already parked"):
            lot.park_entry(car)
