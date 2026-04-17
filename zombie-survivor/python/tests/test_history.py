from .history_builder import HistoryBuilder


def test_new_game_records_a_game_started_event():
    game, _ = HistoryBuilder().build()
    assert len(game.history) == 1
    assert game.history[0].description == "Game started."


def test_adding_a_survivor_records_a_survivor_added_event():
    game, _ = HistoryBuilder().with_survivor("Alice").build()
    assert any(e.description == "Survivor added: Alice." for e in game.history)


def test_receiving_a_wound_records_a_wound_received_event():
    game, _ = (
        HistoryBuilder()
        .with_survivor("Alice")
        .with_wound("Alice")
        .build()
    )
    assert any(e.description == "Wound received: Alice." for e in game.history)


def test_survivor_death_records_a_survivor_died_event():
    game, _ = (
        HistoryBuilder()
        .with_survivor("Alice")
        .with_wound("Alice")
        .with_wound("Alice")
        .build()
    )
    assert any(e.description == "Survivor died: Alice." for e in game.history)


def test_leveling_up_records_a_level_up_event():
    game, _ = (
        HistoryBuilder()
        .with_survivor("Alice")
        .with_zombie_kill("Alice", 7)
        .build()
    )
    assert any(e.description == "Level up: Alice reached Yellow." for e in game.history)


def test_game_level_change_records_a_game_level_changed_event():
    game, _ = (
        HistoryBuilder()
        .with_survivor("Alice")
        .with_zombie_kill("Alice", 7)
        .build()
    )
    assert any(e.description == "Game level changed to Yellow." for e in game.history)


def test_game_ending_records_a_game_ended_event():
    game, _ = (
        HistoryBuilder()
        .with_survivor("Alice")
        .with_wound("Alice")
        .with_wound("Alice")
        .build()
    )
    assert any(e.description == "Game ended." for e in game.history)
