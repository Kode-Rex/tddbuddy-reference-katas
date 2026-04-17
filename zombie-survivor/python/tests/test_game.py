import pytest

from zombie_survivor import DuplicateSurvivorNameException, Survivor

from .history_builder import HistoryBuilder


def test_new_game_starts_with_zero_survivors():
    game, _ = HistoryBuilder().build()
    assert len(game.survivors) == 0


def test_adding_a_survivor_increases_the_survivor_count():
    game, _ = HistoryBuilder().with_survivor("Alice").build()
    assert len(game.survivors) == 1


def test_adding_a_survivor_with_a_duplicate_name_is_rejected():
    game, _ = HistoryBuilder().with_survivor("Alice").build()
    with pytest.raises(
        DuplicateSurvivorNameException,
        match="A survivor named 'Alice' already exists.",
    ):
        game.add_survivor(Survivor("Alice"))


def test_game_ends_when_all_survivors_are_dead():
    game, _ = (
        HistoryBuilder()
        .with_survivor("Alice")
        .with_wound("Alice")
        .with_wound("Alice")
        .build()
    )
    assert game.has_ended is True
