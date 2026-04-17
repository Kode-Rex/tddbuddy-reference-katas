from .clock import Clock
from .exceptions import DuplicateSurvivorNameException
from .history_entry import HistoryEntry
from .level import Level, max_level
from .skill import Skill
from .survivor import Survivor


class Game:
    def __init__(self, clock: Clock) -> None:
        self._clock = clock
        self._survivors: list[Survivor] = []
        self._history: list[HistoryEntry] = []
        self._game_level = Level.BLUE
        self._ended = False
        self._record_event("Game started.")

    @property
    def survivors(self) -> list[Survivor]:
        return list(self._survivors)

    @property
    def history(self) -> list[HistoryEntry]:
        return list(self._history)

    @property
    def game_level(self) -> Level:
        return self._game_level

    @property
    def has_ended(self) -> bool:
        return self._ended

    def add_survivor(self, survivor: Survivor) -> None:
        if any(s.name == survivor.name for s in self._survivors):
            raise DuplicateSurvivorNameException(
                f"A survivor named '{survivor.name}' already exists."
            )
        self._survivors.append(survivor)
        self._record_event(f"Survivor added: {survivor.name}.")

    def wound_survivor(self, survivor: Survivor) -> None:
        was_alive = survivor.is_alive
        changed = survivor.receive_wound()
        if not changed:
            return
        self._record_event(f"Wound received: {survivor.name}.")
        if was_alive and not survivor.is_alive:
            self._record_event(f"Survivor died: {survivor.name}.")
            self._check_game_end()

    def equip_survivor(self, survivor: Survivor, item_name: str) -> None:
        survivor.equip(item_name)
        self._record_event(f"Equipment acquired: {survivor.name} picked up {item_name}.")

    def kill_zombie(self, survivor: Survivor) -> None:
        previous_xp = survivor.experience
        survivor.kill_zombie()
        level_up = survivor.check_level_up(previous_xp)
        if level_up is not None:
            self._record_event(f"Level up: {survivor.name} reached {level_up.value}.")
            if level_up == Level.YELLOW:
                survivor.unlock_skill(Skill.PLUS_ONE_ACTION)
            self._update_game_level()

    def _update_game_level(self) -> None:
        highest = Level.BLUE
        for s in self._survivors:
            if s.is_alive:
                highest = max_level(highest, s.level)
        if highest != self._game_level:
            self._game_level = highest
            self._record_event(f"Game level changed to {self._game_level.value}.")

    def _check_game_end(self) -> None:
        if self._survivors and all(not s.is_alive for s in self._survivors):
            self._ended = True
            self._record_event("Game ended.")

    def _record_event(self, description: str) -> None:
        self._history.append(HistoryEntry(self._clock.now(), description))
