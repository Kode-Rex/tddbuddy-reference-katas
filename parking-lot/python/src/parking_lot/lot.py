from __future__ import annotations

import math
from decimal import Decimal
from typing import Sequence

from .clock import Clock
from .exceptions import (
    InvalidLotConfigurationError,
    InvalidTicketError,
    NoAvailableSpotError,
    VehicleAlreadyParkedError,
)
from .fee import Fee
from .spot_type import SpotType
from .ticket import Ticket
from .vehicle import Vehicle
from .vehicle_type import VehicleType

DEFAULT_MOTORCYCLE_RATE = Decimal("1")
DEFAULT_CAR_RATE = Decimal("3")
DEFAULT_BUS_RATE = Decimal("5")

_MOTORCYCLE_PREFERENCE: Sequence[SpotType] = (SpotType.MOTORCYCLE, SpotType.COMPACT, SpotType.LARGE)
_CAR_PREFERENCE: Sequence[SpotType] = (SpotType.COMPACT, SpotType.LARGE)
_BUS_PREFERENCE: Sequence[SpotType] = (SpotType.LARGE,)

_PREFERENCE_BY_VEHICLE: dict[VehicleType, Sequence[SpotType]] = {
    VehicleType.MOTORCYCLE: _MOTORCYCLE_PREFERENCE,
    VehicleType.CAR: _CAR_PREFERENCE,
    VehicleType.BUS: _BUS_PREFERENCE,
}


class Lot:
    def __init__(
        self,
        motorcycle_spots: int,
        compact_spots: int,
        large_spots: int,
        clock: Clock,
        motorcycle_rate: Decimal = DEFAULT_MOTORCYCLE_RATE,
        car_rate: Decimal = DEFAULT_CAR_RATE,
        bus_rate: Decimal = DEFAULT_BUS_RATE,
    ) -> None:
        if motorcycle_spots + compact_spots + large_spots <= 0:
            raise InvalidLotConfigurationError("Lot must have at least one spot")

        self._clock = clock
        self._available_spots: dict[SpotType, int] = {
            SpotType.MOTORCYCLE: motorcycle_spots,
            SpotType.COMPACT: compact_spots,
            SpotType.LARGE: large_spots,
        }
        self._rates: dict[VehicleType, Decimal] = {
            VehicleType.MOTORCYCLE: motorcycle_rate,
            VehicleType.CAR: car_rate,
            VehicleType.BUS: bus_rate,
        }
        self._active_tickets: dict[str, Ticket] = {}

    def park_entry(self, vehicle: Vehicle) -> Ticket:
        if vehicle.license_plate in self._active_tickets:
            raise VehicleAlreadyParkedError(
                f"Vehicle {vehicle.license_plate} is already parked"
            )

        spot_type = self._allocate_spot(vehicle)
        now = self._clock.now()
        ticket = Ticket(vehicle=vehicle, spot_type=spot_type, entry_time=now)
        self._active_tickets[vehicle.license_plate] = ticket
        return ticket

    def process_exit(self, ticket: Ticket) -> Fee:
        stored = self._active_tickets.get(ticket.vehicle.license_plate)
        if stored is None or stored != ticket:
            raise InvalidTicketError("Ticket is not valid")

        del self._active_tickets[ticket.vehicle.license_plate]
        self._available_spots[ticket.spot_type] += 1

        now = self._clock.now()
        elapsed = now - ticket.entry_time
        total_seconds = elapsed.total_seconds()
        hours = math.ceil(total_seconds / 3600)
        if hours < 1:
            hours = 1

        rate = self._rates[ticket.vehicle.type]
        return Fee(amount=Decimal(hours) * rate)

    def _allocate_spot(self, vehicle: Vehicle) -> SpotType:
        preference = _PREFERENCE_BY_VEHICLE[vehicle.type]

        for spot_type in preference:
            if self._available_spots[spot_type] > 0:
                self._available_spots[spot_type] -= 1
                return spot_type

        raise NoAvailableSpotError(
            f"No available spot for vehicle {vehicle.license_plate}"
        )
