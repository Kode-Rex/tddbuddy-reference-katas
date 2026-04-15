from gilded_rose import GildedRoseInn, Inventory

from .item_builder import ItemBuilder


def test_legendary_items_never_lose_quality():
    item = (
        ItemBuilder()
        .legendary()
        .named("Sulfuras, Hand of Ragnaros")
        .with_quality(80)
        .with_sell_in(5)
        .build()
    )
    inn = GildedRoseInn(Inventory([item]))

    inn.update_inventory()

    assert item.quality == 80


def test_legendary_item_sell_in_never_changes():
    item = (
        ItemBuilder()
        .legendary()
        .named("Sulfuras, Hand of Ragnaros")
        .with_quality(80)
        .with_sell_in(5)
        .build()
    )
    inn = GildedRoseInn(Inventory([item]))

    inn.update_inventory()

    assert item.sell_in == 5


def test_legendary_items_may_have_quality_above_fifty():
    item = (
        ItemBuilder()
        .legendary()
        .named("Sulfuras, Hand of Ragnaros")
        .with_quality(80)
        .with_sell_in(5)
        .build()
    )
    inn = GildedRoseInn(Inventory([item]))

    inn.update_inventory()

    assert item.quality == 80
