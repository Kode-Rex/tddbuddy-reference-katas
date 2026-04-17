from dataclasses import dataclass


@dataclass(frozen=True)
class CombatLog:
    tower_id: str
    jelly_id: str
    damage: int
