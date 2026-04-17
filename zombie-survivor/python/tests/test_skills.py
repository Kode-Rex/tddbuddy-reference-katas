from zombie_survivor import Skill

from .history_builder import HistoryBuilder
from .survivor_builder import SurvivorBuilder


def test_reaching_yellow_unlocks_plus_one_action_as_the_mandatory_first_skill():
    game, _ = (
        HistoryBuilder()
        .with_survivor("Alice")
        .with_zombie_kill("Alice", 7)
        .build()
    )
    alice = next(s for s in game.survivors if s.name == "Alice")
    assert Skill.PLUS_ONE_ACTION in alice.skills


def test_plus_one_action_skill_increases_actions_to_four():
    game, _ = (
        HistoryBuilder()
        .with_survivor("Alice")
        .with_zombie_kill("Alice", 7)
        .build()
    )
    alice = next(s for s in game.survivors if s.name == "Alice")
    assert alice.actions_per_turn == 4


def test_hoard_skill_increases_equipment_capacity_by_one():
    survivor = SurvivorBuilder().build()
    survivor.unlock_skill(Skill.HOARD)
    assert survivor.equipment_capacity == 6
