from datetime import date

from clam_card import Card, Ride, Zone
from decimal import Decimal


class CardBuilder:
    """Test-folder synthesiser. Stages a card with a known set of
    zone-tagged stations. ``on_day`` is accepted for scenario readability;
    daily caps reset on card construction in this reference, so the date
    has no behaviour at F2 scope.
    """

    def __init__(self) -> None:
        self._stations: dict[str, Zone] = {}

    def on_day(self, _d: date) -> "CardBuilder":
        return self

    def with_zone(self, zone: Zone, *stations: str) -> "CardBuilder":
        for s in stations:
            self._stations[s] = zone
        return self

    def build(self) -> Card:
        return Card(self._stations)


class RideBuilder:
    """Test-folder synthesiser for ``Ride`` literals used in equality
    assertions against ``card.rides()``.
    """

    def __init__(self) -> None:
        self._from: str = ""
        self._to: str = ""
        self._zone: Zone = Zone.A
        self._fare: Decimal = Decimal("0")

    def from_station(self, station: str) -> "RideBuilder":
        self._from = station
        return self

    def to(self, station: str) -> "RideBuilder":
        self._to = station
        return self

    def charged_at(self, zone: Zone) -> "RideBuilder":
        self._zone = zone
        return self

    def with_fare(self, fare: Decimal) -> "RideBuilder":
        self._fare = fare
        return self

    def build(self) -> Ride:
        return Ride(self._from, self._to, self._zone, self._fare)
