from gilded_rose import GildedRoseInn, Inventory

from .item_builder import ItemBuilder


def test_aged_items_gain_one_quality_per_day_while_fresh():
    item = ItemBuilder().aged().with_quality(10).with_sell_in(5).build()
    inn = GildedRoseInn(Inventory([item]))

    inn.update_inventory()

    assert item.quality == 11


def test_aged_items_gain_two_quality_per_day_after_the_sell_by_date():
    item = ItemBuilder().aged().with_quality(10).with_sell_in(0).build()
    inn = GildedRoseInn(Inventory([item]))

    inn.update_inventory()

    assert item.quality == 12


def test_aged_item_quality_never_exceeds_fifty():
    item = ItemBuilder().aged().with_quality(50).with_sell_in(5).build()
    inn = GildedRoseInn(Inventory([item]))

    inn.update_inventory()

    assert item.quality == 50


def test_aged_item_sell_in_decreases_by_one_each_day():
    item = ItemBuilder().aged().with_quality(10).with_sell_in(5).build()
    inn = GildedRoseInn(Inventory([item]))

    inn.update_inventory()

    assert item.sell_in == 4
