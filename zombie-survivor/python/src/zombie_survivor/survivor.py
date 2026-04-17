from .equipment import Equipment, EquipmentSlot
from .exceptions import EquipmentCapacityExceededException
from .level import Level, level_for
from .skill import Skill

_BASE_ACTIONS = 3
_MAX_WOUNDS = 2
_BASE_CAPACITY = 5
_MAX_IN_HAND = 2


class Survivor:
    def __init__(self, name: str) -> None:
        self._name = name
        self._wounds = 0
        self._experience = 0
        self._equipment: list[Equipment] = []
        self._skills: list[Skill] = []

    @property
    def name(self) -> str:
        return self._name

    @property
    def wounds(self) -> int:
        return self._wounds

    @property
    def experience(self) -> int:
        return self._experience

    @property
    def is_alive(self) -> bool:
        return self._wounds < _MAX_WOUNDS

    @property
    def level(self) -> Level:
        return level_for(self._experience)

    @property
    def actions_per_turn(self) -> int:
        bonus = sum(1 for s in self._skills if s == Skill.PLUS_ONE_ACTION)
        return _BASE_ACTIONS + bonus

    @property
    def equipment_capacity(self) -> int:
        hoard_bonus = sum(1 for s in self._skills if s == Skill.HOARD)
        return _BASE_CAPACITY - self._wounds + hoard_bonus

    @property
    def equipment(self) -> list[Equipment]:
        return list(self._equipment)

    @property
    def skills(self) -> list[Skill]:
        return list(self._skills)

    @property
    def in_hand_count(self) -> int:
        return sum(1 for e in self._equipment if e.slot == EquipmentSlot.IN_HAND)

    @property
    def in_reserve_count(self) -> int:
        return sum(1 for e in self._equipment if e.slot == EquipmentSlot.IN_RESERVE)

    def receive_wound(self) -> bool:
        if not self.is_alive:
            return False
        self._wounds += 1
        return True

    def equip(self, item_name: str) -> None:
        if len(self._equipment) >= self.equipment_capacity:
            raise EquipmentCapacityExceededException(
                f"Cannot carry more than {self.equipment_capacity} pieces of equipment."
            )
        slot = EquipmentSlot.IN_HAND if self.in_hand_count < _MAX_IN_HAND else EquipmentSlot.IN_RESERVE
        self._equipment.append(Equipment(item_name, slot))

    def discard(self, item_name: str) -> None:
        for i, e in enumerate(self._equipment):
            if e.name == item_name:
                self._equipment.pop(i)
                return

    @property
    def needs_to_discard(self) -> bool:
        return len(self._equipment) > self.equipment_capacity

    def kill_zombie(self) -> None:
        self._experience += 1

    def check_level_up(self, previous_experience: int) -> Level | None:
        previous_level = level_for(previous_experience)
        if self.level != previous_level:
            return self.level
        return None

    def unlock_skill(self, skill: Skill) -> None:
        self._skills.append(skill)
