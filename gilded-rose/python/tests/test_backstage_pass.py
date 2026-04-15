from gilded_rose import GildedRoseInn, Inventory

from .item_builder import ItemBuilder


def test_backstage_pass_quality_increases_by_one_when_concert_is_more_than_ten_days_away():
    item = ItemBuilder().backstage_pass().with_quality(10).with_sell_in(15).build()
    inn = GildedRoseInn(Inventory([item]))

    inn.update_inventory()

    assert item.quality == 11
