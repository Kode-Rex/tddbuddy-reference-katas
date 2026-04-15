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
