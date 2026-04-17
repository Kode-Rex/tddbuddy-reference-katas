from datetime import timedelta
from decimal import Decimal

import pytest

from parking_lot import NoAvailableSpotError, SpotType
from tests.lot_builder import LotBuilder
from tests.vehicle_builder import VehicleBuilder


class TestWorkedExample:
    def test_end_to_end_lot_with_one_of_each_spot_type(self) -> None:
        lot, clock = (
            LotBuilder()
            .with_motorcycle_spots(1)
            .with_compact_spots(1)
            .with_large_spots(1)
            .build()
        )

        motorcycle = VehicleBuilder().as_motorcycle().with_plate("MC-001").build()
        car = VehicleBuilder().as_car().with_plate("CAR-001").build()
        bus = VehicleBuilder().as_bus().with_plate("BUS-001").build()

        # Park all three — fills the lot
        mc_ticket = lot.park_entry(motorcycle)
        assert mc_ticket.spot_type == SpotType.MOTORCYCLE

        car_ticket = lot.park_entry(car)
        assert car_ticket.spot_type == SpotType.COMPACT

        bus_ticket = lot.park_entry(bus)
        assert bus_ticket.spot_type == SpotType.LARGE

        # Exit motorcycle at t+90min -> $2 (ceil(1.5) = 2 hours x $1)
        clock.advance(timedelta(minutes=90))
        mc_fee = lot.process_exit(mc_ticket)
        assert mc_fee.amount == Decimal("2")

        # Try to park a second car — motorcycle spot freed, but car doesn't fit motorcycle spot
        car2 = VehicleBuilder().as_car().with_plate("CAR-002").build()
        with pytest.raises(NoAvailableSpotError, match="No available spot for vehicle CAR-002"):
            lot.park_entry(car2)

        # Park a second motorcycle — gets the freed motorcycle spot
        mc2 = VehicleBuilder().as_motorcycle().with_plate("MC-002").build()
        mc2_ticket = lot.park_entry(mc2)
        assert mc2_ticket.spot_type == SpotType.MOTORCYCLE

        # Exit car at t+120min from start -> $6 (2 hours x $3)
        clock.advance(timedelta(minutes=30))
        car_fee = lot.process_exit(car_ticket)
        assert car_fee.amount == Decimal("6")

        # Exit bus at t+2h from start -> $10 (2 hours x $5)
        bus_fee = lot.process_exit(bus_ticket)
        assert bus_fee.amount == Decimal("10")
