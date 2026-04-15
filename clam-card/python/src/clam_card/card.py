from __future__ import annotations

from dataclasses import dataclass, field
from decimal import Decimal
from enum import Enum
from typing import Dict, List


# Identical byte-for-byte across C#, TypeScript, and Python.
# The exception messages are the spec (see ../SCENARIOS.md).
class CardMessages:
    UNKNOWN_STATION = "station is not on this card's network"


ZONE_A_SINGLE_FARE = Decimal("2.50")
ZONE_B_SINGLE_FARE = Decimal("3.00")
ZONE_A_DAILY_CAP = Decimal("7.00")
ZONE_B_DAILY_CAP = Decimal("8.00")


class Zone(Enum):
    A = "A"
    B = "B"


class UnknownStationError(ValueError):
    def __init__(self) -> None:
        super().__init__(CardMessages.UNKNOWN_STATION)


@dataclass(frozen=True)
class Ride:
    from_station: str
    to_station: str
    zone: Zone
    fare: Decimal


class Card:
    def __init__(self, stations: Dict[str, Zone]) -> None:
        """Test-folder constructor hook; production code would inject a real network."""
        self._stations: Dict[str, Zone] = dict(stations)
        self._rides: List[Ride] = []
        self._charged_zone_a_today: Decimal = Decimal("0.00")
        self._charged_zone_b_today: Decimal = Decimal("0.00")

    def rides(self) -> List[Ride]:
        return list(self._rides)

    def total_charged(self) -> Decimal:
        return self._charged_zone_a_today + self._charged_zone_b_today

    def travel_from(self, station: str) -> "JourneyStart":
        self._ensure_known(station)
        return JourneyStart(self, station)

    def _complete_journey(self, from_station: str, to_station: str) -> Ride:
        self._ensure_known(to_station)
        from_zone = self._stations[from_station]
        to_zone = self._stations[to_station]
        journey_zone = Zone.B if Zone.B in (from_zone, to_zone) else Zone.A

        fare = self._charge_for_zone(journey_zone)
        ride = Ride(from_station, to_station, journey_zone, fare)
        self._rides.append(ride)
        return ride

    def _charge_for_zone(self, zone: Zone) -> Decimal:
        if zone == Zone.A:
            single_fare = ZONE_A_SINGLE_FARE
            cap = ZONE_A_DAILY_CAP
            charged = self._charged_zone_a_today
        else:
            single_fare = ZONE_B_SINGLE_FARE
            cap = ZONE_B_DAILY_CAP
            charged = self._charged_zone_b_today

        remaining = cap - charged
        fare = max(Decimal("0"), min(single_fare, remaining))

        if zone == Zone.A:
            self._charged_zone_a_today += fare
        else:
            self._charged_zone_b_today += fare

        return fare

    def _ensure_known(self, station: str) -> None:
        if station not in self._stations:
            raise UnknownStationError()


@dataclass
class JourneyStart:
    _card: Card = field(repr=False)
    _from: str

    def to(self, station: str) -> Ride:
        return self._card._complete_journey(self._from, station)
