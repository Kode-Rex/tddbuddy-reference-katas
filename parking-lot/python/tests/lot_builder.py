from __future__ import annotations

from datetime import datetime, timezone
from decimal import Decimal

from parking_lot import DEFAULT_BUS_RATE, DEFAULT_CAR_RATE, DEFAULT_MOTORCYCLE_RATE, Lot
from tests.fixed_clock import FixedClock


class LotBuilder:
    def __init__(self) -> None:
        self._motorcycle_spots = 0
        self._compact_spots = 0
        self._large_spots = 0
        self._motorcycle_rate = DEFAULT_MOTORCYCLE_RATE
        self._car_rate = DEFAULT_CAR_RATE
        self._bus_rate = DEFAULT_BUS_RATE
        self._start = datetime(2026, 1, 1, tzinfo=timezone.utc)
        self._clock: FixedClock | None = None

    def with_motorcycle_spots(self, count: int) -> LotBuilder:
        self._motorcycle_spots = count
        return self

    def with_compact_spots(self, count: int) -> LotBuilder:
        self._compact_spots = count
        return self

    def with_large_spots(self, count: int) -> LotBuilder:
        self._large_spots = count
        return self

    def with_motorcycle_rate(self, rate: Decimal) -> LotBuilder:
        self._motorcycle_rate = rate
        return self

    def with_car_rate(self, rate: Decimal) -> LotBuilder:
        self._car_rate = rate
        return self

    def with_bus_rate(self, rate: Decimal) -> LotBuilder:
        self._bus_rate = rate
        return self

    def starting_at(self, start: datetime) -> LotBuilder:
        self._start = start
        return self

    def with_clock(self, clock: FixedClock) -> LotBuilder:
        self._clock = clock
        return self

    def build(self) -> tuple[Lot, FixedClock]:
        clock = self._clock or FixedClock(self._start)
        lot = Lot(
            self._motorcycle_spots,
            self._compact_spots,
            self._large_spots,
            clock,
            self._motorcycle_rate,
            self._car_rate,
            self._bus_rate,
        )
        return lot, clock
