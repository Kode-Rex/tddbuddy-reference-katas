from zombie_survivor import Level

from .survivor_builder import SurvivorBuilder


def test_new_survivor_has_zero_wounds():
    survivor = SurvivorBuilder().build()
    assert survivor.wounds == 0


def test_new_survivor_has_three_actions_per_turn():
    survivor = SurvivorBuilder().build()
    assert survivor.actions_per_turn == 3


def test_new_survivor_is_alive():
    survivor = SurvivorBuilder().build()
    assert survivor.is_alive is True


def test_new_survivor_starts_at_level_blue():
    survivor = SurvivorBuilder().build()
    assert survivor.level == Level.BLUE


def test_receiving_a_wound_leaves_the_survivor_alive_with_one_wound():
    survivor = SurvivorBuilder().build()
    survivor.receive_wound()
    assert survivor.wounds == 1
    assert survivor.is_alive is True


def test_receiving_a_second_wound_kills_the_survivor():
    survivor = SurvivorBuilder().with_wounds(1).build()
    survivor.receive_wound()
    assert survivor.is_alive is False
    assert survivor.wounds == 2


def test_wounding_a_dead_survivor_has_no_effect():
    survivor = SurvivorBuilder().with_wounds(2).build()
    survivor.receive_wound()
    assert survivor.wounds == 2
