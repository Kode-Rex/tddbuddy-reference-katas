from gilded_rose import GildedRoseInn, Inventory

from .item_builder import ItemBuilder


def test_standard_items_lose_one_quality_per_day_while_fresh():
    item = ItemBuilder().standard().with_quality(10).with_sell_in(5).build()
    inn = GildedRoseInn(Inventory([item]))

    inn.update_inventory()

    assert item.quality == 9
