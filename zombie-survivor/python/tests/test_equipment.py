import pytest

from zombie_survivor import EquipmentCapacityExceededException

from .survivor_builder import SurvivorBuilder


def test_new_survivor_can_carry_up_to_five_pieces_of_equipment():
    survivor = SurvivorBuilder().build()
    assert survivor.equipment_capacity == 5


def test_survivor_can_hold_up_to_two_items_in_hand():
    survivor = SurvivorBuilder().with_equipment("Bat", "Pistol").build()
    assert survivor.in_hand_count == 2


def test_remaining_equipment_goes_in_reserve():
    survivor = SurvivorBuilder().with_equipment("Bat", "Pistol", "Medkit").build()
    assert survivor.in_hand_count == 2
    assert survivor.in_reserve_count == 1


def test_equipping_a_sixth_item_is_rejected():
    survivor = SurvivorBuilder().with_equipment(
        "Bat", "Pistol", "Medkit", "Axe", "Shield"
    ).build()
    with pytest.raises(
        EquipmentCapacityExceededException,
        match="Cannot carry more than 5 pieces of equipment.",
    ):
        survivor.equip("Grenade")


def test_one_wound_reduces_carrying_capacity_to_four():
    survivor = SurvivorBuilder().with_wounds(1).build()
    assert survivor.equipment_capacity == 4


def test_wound_with_full_equipment_requires_discarding_one_item():
    survivor = SurvivorBuilder().with_equipment(
        "Bat", "Pistol", "Medkit", "Axe", "Shield"
    ).build()
    survivor.receive_wound()
    assert survivor.needs_to_discard is True
    survivor.discard("Shield")
    assert survivor.needs_to_discard is False
    assert len(survivor.equipment) == 4
