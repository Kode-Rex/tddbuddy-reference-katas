from datetime import datetime, timezone
from typing import Callable

from zombie_survivor import Game, Survivor

from .fixed_clock import FixedClock

GameAction = Callable[[Game], None]


class HistoryBuilder:
    def __init__(self) -> None:
        self._start_time = datetime(2026, 1, 1, 12, 0, 0, tzinfo=timezone.utc)
        self._actions: list[GameAction] = []

    def with_survivor(self, name: str) -> "HistoryBuilder":
        self._actions.append(lambda game: game.add_survivor(Survivor(name)))
        return self

    def with_wound(self, survivor_name: str) -> "HistoryBuilder":
        def action(game: Game) -> None:
            survivor = next(s for s in game.survivors if s.name == survivor_name)
            game.wound_survivor(survivor)
        self._actions.append(action)
        return self

    def with_zombie_kill(self, survivor_name: str, count: int = 1) -> "HistoryBuilder":
        def action(game: Game) -> None:
            survivor = next(s for s in game.survivors if s.name == survivor_name)
            for _ in range(count):
                game.kill_zombie(survivor)
        self._actions.append(action)
        return self

    def build(self) -> tuple[Game, FixedClock]:
        clock = FixedClock(self._start_time)
        game = Game(clock)
        for action in self._actions:
            action(game)
        return game, clock
