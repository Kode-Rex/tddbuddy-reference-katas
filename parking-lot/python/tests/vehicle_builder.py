from __future__ import annotations

from parking_lot import Vehicle, VehicleType


class VehicleBuilder:
    def __init__(self) -> None:
        self._type = VehicleType.CAR
        self._plate = "CAR-001"

    def as_motorcycle(self) -> VehicleBuilder:
        self._type = VehicleType.MOTORCYCLE
        self._plate = "MC-001"
        return self

    def as_car(self) -> VehicleBuilder:
        self._type = VehicleType.CAR
        self._plate = "CAR-001"
        return self

    def as_bus(self) -> VehicleBuilder:
        self._type = VehicleType.BUS
        self._plate = "BUS-001"
        return self

    def with_plate(self, plate: str) -> VehicleBuilder:
        self._plate = plate
        return self

    def build(self) -> Vehicle:
        return Vehicle(type=self._type, license_plate=self._plate)
