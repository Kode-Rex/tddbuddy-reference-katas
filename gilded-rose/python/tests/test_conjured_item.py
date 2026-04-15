from gilded_rose import GildedRoseInn, Inventory

from .item_builder import ItemBuilder


def test_conjured_items_lose_two_quality_per_day_while_fresh():
    item = ItemBuilder().conjured().with_quality(10).with_sell_in(5).build()
    inn = GildedRoseInn(Inventory([item]))

    inn.update_inventory()

    assert item.quality == 8


def test_conjured_items_lose_four_quality_per_day_after_the_sell_by_date():
    item = ItemBuilder().conjured().with_quality(10).with_sell_in(0).build()
    inn = GildedRoseInn(Inventory([item]))

    inn.update_inventory()

    assert item.quality == 6


def test_conjured_item_quality_never_goes_below_zero():
    item = ItemBuilder().conjured().with_quality(1).with_sell_in(5).build()
    inn = GildedRoseInn(Inventory([item]))

    inn.update_inventory()

    assert item.quality == 0
