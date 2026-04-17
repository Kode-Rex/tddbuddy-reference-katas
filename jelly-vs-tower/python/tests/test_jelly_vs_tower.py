import pytest

from jelly_vs_tower import (
    Arena,
    ColorType,
    InvalidHealthException,
    InvalidLevelException,
    calculate_damage,
)

from .fixed_random_source import FixedRandomSource
from .jelly_builder import JellyBuilder
from .tower_builder import TowerBuilder


# ── Jelly Creation and Health ──────────────────────────────


def test_jelly_starts_alive_with_the_given_health():
    jelly = JellyBuilder().with_health(20).build()
    assert jelly.health == 20
    assert jelly.is_alive is True


def test_jelly_with_zero_health_is_rejected():
    with pytest.raises(InvalidHealthException, match="Health must be strictly positive, got 0"):
        JellyBuilder().with_health(0).build()


def test_jelly_with_negative_health_is_rejected():
    with pytest.raises(InvalidHealthException, match="Health must be strictly positive, got -5"):
        JellyBuilder().with_health(-5).build()


def test_jelly_dies_when_health_reaches_zero():
    jelly = JellyBuilder().with_health(5).build()
    jelly.take_damage(5)
    assert jelly.is_alive is False
    assert jelly.health == 0


def test_jelly_dies_when_health_drops_below_zero():
    jelly = JellyBuilder().with_health(3).build()
    jelly.take_damage(10)
    assert jelly.is_alive is False
    assert jelly.health == 0


# ── Tower Creation and Validation ─────────────────────────


def test_tower_is_created_with_a_color_and_level():
    tower = TowerBuilder().with_color(ColorType.Red).with_level(3).build()
    assert tower.color == ColorType.Red
    assert tower.level == 3


def test_tower_with_level_below_1_is_rejected():
    with pytest.raises(InvalidLevelException, match="Tower level must be between 1 and 4, got 0"):
        TowerBuilder().with_level(0).build()


def test_tower_with_level_above_4_is_rejected():
    with pytest.raises(InvalidLevelException, match="Tower level must be between 1 and 4, got 5"):
        TowerBuilder().with_level(5).build()


# ── Damage Lookup — Blue Tower ────────────────────────────


def test_blue_tower_level_1_deals_2_to_5_damage_to_a_blue_jelly():
    tower = TowerBuilder().with_color(ColorType.Blue).with_level(1).build()
    jelly = JellyBuilder().with_color(ColorType.Blue).with_health(100).build()
    assert calculate_damage(tower, jelly, FixedRandomSource(2)) == 2
    assert calculate_damage(tower, jelly, FixedRandomSource(5)) == 5


def test_blue_tower_level_1_deals_0_damage_to_a_red_jelly():
    tower = TowerBuilder().with_color(ColorType.Blue).with_level(1).build()
    jelly = JellyBuilder().with_color(ColorType.Red).with_health(100).build()
    assert calculate_damage(tower, jelly, FixedRandomSource(0)) == 0


def test_blue_tower_level_4_deals_12_to_15_damage_to_a_blue_jelly():
    tower = TowerBuilder().with_color(ColorType.Blue).with_level(4).build()
    jelly = JellyBuilder().with_color(ColorType.Blue).with_health(100).build()
    assert calculate_damage(tower, jelly, FixedRandomSource(12)) == 12
    assert calculate_damage(tower, jelly, FixedRandomSource(15)) == 15


def test_blue_tower_level_2_deals_1_damage_to_a_red_jelly():
    tower = TowerBuilder().with_color(ColorType.Blue).with_level(2).build()
    jelly = JellyBuilder().with_color(ColorType.Red).with_health(100).build()
    assert calculate_damage(tower, jelly, FixedRandomSource(1)) == 1


# ── Damage Lookup — Red Tower ─────────────────────────────


def test_red_tower_level_3_deals_9_to_12_damage_to_a_red_jelly():
    tower = TowerBuilder().with_color(ColorType.Red).with_level(3).build()
    jelly = JellyBuilder().with_color(ColorType.Red).with_health(100).build()
    assert calculate_damage(tower, jelly, FixedRandomSource(9)) == 9
    assert calculate_damage(tower, jelly, FixedRandomSource(12)) == 12


def test_red_tower_level_2_deals_1_damage_to_a_blue_jelly():
    tower = TowerBuilder().with_color(ColorType.Red).with_level(2).build()
    jelly = JellyBuilder().with_color(ColorType.Blue).with_health(100).build()
    assert calculate_damage(tower, jelly, FixedRandomSource(1)) == 1


def test_red_tower_level_1_deals_0_damage_to_a_blue_jelly():
    tower = TowerBuilder().with_color(ColorType.Red).with_level(1).build()
    jelly = JellyBuilder().with_color(ColorType.Blue).with_health(100).build()
    assert calculate_damage(tower, jelly, FixedRandomSource(0)) == 0


# ── Damage Lookup — BlueRed Tower ─────────────────────────


def test_bluered_tower_level_4_deals_6_to_8_damage_to_a_blue_jelly():
    tower = TowerBuilder().with_color(ColorType.BlueRed).with_level(4).build()
    jelly = JellyBuilder().with_color(ColorType.Blue).with_health(100).build()
    assert calculate_damage(tower, jelly, FixedRandomSource(6)) == 6
    assert calculate_damage(tower, jelly, FixedRandomSource(8)) == 8


def test_bluered_tower_level_4_deals_6_to_8_damage_to_a_red_jelly():
    tower = TowerBuilder().with_color(ColorType.BlueRed).with_level(4).build()
    jelly = JellyBuilder().with_color(ColorType.Red).with_health(100).build()
    assert calculate_damage(tower, jelly, FixedRandomSource(6)) == 6
    assert calculate_damage(tower, jelly, FixedRandomSource(8)) == 8


def test_bluered_tower_level_1_deals_2_damage_to_a_blue_jelly():
    tower = TowerBuilder().with_color(ColorType.BlueRed).with_level(1).build()
    jelly = JellyBuilder().with_color(ColorType.Blue).with_health(100).build()
    assert calculate_damage(tower, jelly, FixedRandomSource(2)) == 2


# ── BlueRed Jelly — Takes Higher Column ───────────────────


def test_bluered_jelly_takes_the_higher_of_blue_and_red_column_damage():
    tower = TowerBuilder().with_color(ColorType.Blue).with_level(1).build()
    jelly = JellyBuilder().with_color(ColorType.BlueRed).with_health(100).build()
    assert calculate_damage(tower, jelly, FixedRandomSource(3)) == 3


def test_bluered_jelly_hit_by_bluered_tower_uses_both_columns_and_takes_the_higher():
    tower = TowerBuilder().with_color(ColorType.BlueRed).with_level(2).build()
    jelly = JellyBuilder().with_color(ColorType.BlueRed).with_health(100).build()
    assert calculate_damage(tower, jelly, FixedRandomSource(3)) == 3


# ── Combat Flow ───────────────────────────────────────────


def test_tower_attacks_the_first_alive_jelly_and_produces_a_combat_log():
    tower = TowerBuilder().with_id("T1").with_color(ColorType.Blue).with_level(1).build()
    jelly = JellyBuilder().with_id("J1").with_color(ColorType.Blue).with_health(20).build()
    arena = Arena([tower], [jelly], FixedRandomSource(3))
    logs = arena.execute_round()
    assert len(logs) == 1
    assert logs[0].tower_id == "T1"
    assert logs[0].jelly_id == "J1"
    assert logs[0].damage == 3
    assert jelly.health == 17


def test_dead_jellies_are_skipped_tower_attacks_the_next_alive_jelly():
    tower = TowerBuilder().with_id("T1").with_color(ColorType.Blue).with_level(4).build()
    dead_jelly = JellyBuilder().with_id("J1").with_color(ColorType.Blue).with_health(1).build()
    dead_jelly.take_damage(1)
    alive_jelly = JellyBuilder().with_id("J2").with_color(ColorType.Blue).with_health(20).build()
    arena = Arena([tower], [dead_jelly, alive_jelly], FixedRandomSource(12))
    logs = arena.execute_round()
    assert len(logs) == 1
    assert logs[0].jelly_id == "J2"


def test_tower_attack_does_nothing_when_no_alive_jellies_remain():
    tower = TowerBuilder().with_color(ColorType.Blue).with_level(1).build()
    jelly = JellyBuilder().with_health(1).build()
    jelly.take_damage(1)
    arena = Arena([tower], [jelly], FixedRandomSource(3))
    logs = arena.execute_round()
    assert len(logs) == 0


def test_multiple_towers_each_attack_in_a_single_round():
    tower1 = TowerBuilder().with_id("T1").with_color(ColorType.Blue).with_level(1).build()
    tower2 = TowerBuilder().with_id("T2").with_color(ColorType.Red).with_level(1).build()
    jelly = JellyBuilder().with_id("J1").with_color(ColorType.Blue).with_health(100).build()
    arena = Arena([tower1, tower2], [jelly], FixedRandomSource(3))
    logs = arena.execute_round()
    assert len(logs) == 2
    assert logs[0].tower_id == "T1"
    assert logs[1].tower_id == "T2"


def test_jelly_killed_during_a_round_is_removed_before_the_next_tower_attacks():
    tower1 = TowerBuilder().with_id("T1").with_color(ColorType.Blue).with_level(4).build()
    tower2 = TowerBuilder().with_id("T2").with_color(ColorType.Blue).with_level(1).build()
    jelly1 = JellyBuilder().with_id("J1").with_color(ColorType.Blue).with_health(1).build()
    jelly2 = JellyBuilder().with_id("J2").with_color(ColorType.Blue).with_health(20).build()
    arena = Arena([tower1, tower2], [jelly1, jelly2], FixedRandomSource(12))
    logs = arena.execute_round()
    assert len(logs) == 2
    assert logs[0].jelly_id == "J1"
    assert logs[1].jelly_id == "J2"
    assert jelly1.is_alive is False
