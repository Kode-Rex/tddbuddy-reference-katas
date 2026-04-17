from datetime import datetime, timedelta, timezone

import pytest

from parking_lot import InvalidTicketError, SpotType, Ticket, Vehicle, VehicleType
from tests.lot_builder import LotBuilder
from tests.vehicle_builder import VehicleBuilder


class TestExit:
    def test_exiting_frees_the_spot_so_another_vehicle_of_the_same_type_can_park(self) -> None:
        lot, clock = LotBuilder().with_motorcycle_spots(0).with_compact_spots(1).with_large_spots(0).build()
        car1 = VehicleBuilder().as_car().with_plate("CAR-001").build()
        car2 = VehicleBuilder().as_car().with_plate("CAR-002").build()

        ticket = lot.park_entry(car1)
        clock.advance(timedelta(hours=1))
        lot.process_exit(ticket)

        lot.park_entry(car2)  # should not raise

    def test_exiting_with_an_invalid_ticket_raises_invalid_ticket_error(self) -> None:
        lot, _ = LotBuilder().with_motorcycle_spots(1).with_compact_spots(1).with_large_spots(1).build()
        fake_ticket = Ticket(
            vehicle=Vehicle(VehicleType.CAR, "FAKE-001"),
            spot_type=SpotType.COMPACT,
            entry_time=datetime(2026, 1, 1, tzinfo=timezone.utc),
        )

        with pytest.raises(InvalidTicketError, match="Ticket is not valid"):
            lot.process_exit(fake_ticket)

    def test_exiting_with_the_same_ticket_twice_raises_invalid_ticket_error(self) -> None:
        lot, clock = LotBuilder().with_motorcycle_spots(1).with_compact_spots(1).with_large_spots(1).build()
        car = VehicleBuilder().as_car().build()
        ticket = lot.park_entry(car)

        clock.advance(timedelta(hours=1))
        lot.process_exit(ticket)

        with pytest.raises(InvalidTicketError, match="Ticket is not valid"):
            lot.process_exit(ticket)
