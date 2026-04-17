from __future__ import annotations

from dataclasses import dataclass

from .vehicle_type import VehicleType


@dataclass(frozen=True)
class Vehicle:
    type: VehicleType
    license_plate: str
