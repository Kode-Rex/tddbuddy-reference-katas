from datetime import timedelta
from decimal import Decimal

from tests.lot_builder import LotBuilder
from tests.vehicle_builder import VehicleBuilder


class TestFeeCalculation:
    def test_a_motorcycle_parked_for_exactly_one_hour_pays_1_dollar(self) -> None:
        lot, clock = LotBuilder().with_motorcycle_spots(1).build()
        motorcycle = VehicleBuilder().as_motorcycle().build()

        ticket = lot.park_entry(motorcycle)
        clock.advance(timedelta(hours=1))
        fee = lot.process_exit(ticket)

        assert fee.amount == Decimal("1")

    def test_a_car_parked_for_exactly_two_hours_pays_6_dollars(self) -> None:
        lot, clock = LotBuilder().with_compact_spots(1).build()
        car = VehicleBuilder().as_car().build()

        ticket = lot.park_entry(car)
        clock.advance(timedelta(hours=2))
        fee = lot.process_exit(ticket)

        assert fee.amount == Decimal("6")

    def test_a_bus_parked_for_exactly_one_hour_pays_5_dollars(self) -> None:
        lot, clock = LotBuilder().with_large_spots(1).build()
        bus = VehicleBuilder().as_bus().build()

        ticket = lot.park_entry(bus)
        clock.advance(timedelta(hours=1))
        fee = lot.process_exit(ticket)

        assert fee.amount == Decimal("5")

    def test_partial_hours_round_up_a_car_parked_for_2_hours_1_minute_pays_9_dollars(self) -> None:
        lot, clock = LotBuilder().with_compact_spots(1).build()
        car = VehicleBuilder().as_car().build()

        ticket = lot.park_entry(car)
        clock.advance(timedelta(hours=2, minutes=1))
        fee = lot.process_exit(ticket)

        assert fee.amount == Decimal("9")

    def test_a_stay_of_zero_duration_is_billed_as_1_hour(self) -> None:
        lot, _ = LotBuilder().with_compact_spots(1).build()
        car = VehicleBuilder().as_car().build()

        ticket = lot.park_entry(car)
        fee = lot.process_exit(ticket)

        assert fee.amount == Decimal("3")
