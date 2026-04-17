from __future__ import annotations

from .combat_log import CombatLog
from .damage_table import calculate_damage
from .jelly import Jelly
from .random_source import RandomSource
from .tower import Tower


class Arena:
    def __init__(
        self,
        towers: list[Tower],
        jellies: list[Jelly],
        random: RandomSource,
    ) -> None:
        self._towers = list(towers)
        self._jellies = list(jellies)
        self._random = random

    @property
    def alive_jellies(self) -> list[Jelly]:
        return [j for j in self._jellies if j.is_alive]

    def execute_round(self) -> list[CombatLog]:
        logs: list[CombatLog] = []

        for tower in self._towers:
            target = next((j for j in self._jellies if j.is_alive), None)
            if target is None:
                break

            damage = calculate_damage(tower, target, self._random)
            target.take_damage(damage)
            logs.append(CombatLog(tower.id, target.id, damage))

        return logs
