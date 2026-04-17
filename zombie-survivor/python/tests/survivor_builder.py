from zombie_survivor import Survivor


class SurvivorBuilder:
    def __init__(self) -> None:
        self._name = "Bob"
        self._wounds = 0
        self._zombie_kills = 0
        self._equipment: list[str] = []

    def named(self, name: str) -> "SurvivorBuilder":
        self._name = name
        return self

    def with_wounds(self, wounds: int) -> "SurvivorBuilder":
        self._wounds = wounds
        return self

    def with_zombie_kills(self, kills: int) -> "SurvivorBuilder":
        self._zombie_kills = kills
        return self

    def with_equipment(self, *items: str) -> "SurvivorBuilder":
        self._equipment.extend(items)
        return self

    def build(self) -> Survivor:
        survivor = Survivor(self._name)
        for _ in range(self._zombie_kills):
            survivor.kill_zombie()
        for item in self._equipment:
            survivor.equip(item)
        for _ in range(self._wounds):
            survivor.receive_wound()
        return survivor
