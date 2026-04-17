from zombie_survivor import Level

from .history_builder import HistoryBuilder
from .survivor_builder import SurvivorBuilder


def test_killing_a_zombie_awards_one_experience_point():
    survivor = SurvivorBuilder().build()
    survivor.kill_zombie()
    assert survivor.experience == 1


def test_survivor_with_seven_experience_is_level_yellow():
    survivor = SurvivorBuilder().with_zombie_kills(7).build()
    assert survivor.level == Level.YELLOW


def test_survivor_with_nineteen_experience_is_level_orange():
    survivor = SurvivorBuilder().with_zombie_kills(19).build()
    assert survivor.level == Level.ORANGE


def test_survivor_with_forty_three_experience_is_level_red():
    survivor = SurvivorBuilder().with_zombie_kills(43).build()
    assert survivor.level == Level.RED


def test_game_level_matches_the_highest_level_among_living_survivors():
    game, _ = (
        HistoryBuilder()
        .with_survivor("Alice")
        .with_survivor("Bob")
        .with_zombie_kill("Alice", 7)
        .build()
    )
    assert game.game_level == Level.YELLOW
