from dataclasses import dataclass
from enum import Enum


class EquipmentSlot(Enum):
    IN_HAND = "InHand"
    IN_RESERVE = "InReserve"


@dataclass(frozen=True)
class Equipment:
    name: str
    slot: EquipmentSlot
