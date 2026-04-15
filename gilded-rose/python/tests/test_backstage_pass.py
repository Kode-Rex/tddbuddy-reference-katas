from gilded_rose import GildedRoseInn, Inventory

from .item_builder import ItemBuilder


def test_backstage_pass_quality_increases_by_one_when_concert_is_more_than_ten_days_away():
    item = (
        ItemBuilder()
        .backstage_pass()
        .with_quality(10)
        .with_sell_in(15)
        .build()
    )
    inn = GildedRoseInn(Inventory([item]))

    inn.update_inventory()

    assert item.quality == 11


def test_backstage_pass_quality_increases_by_two_when_concert_is_ten_days_or_fewer_away():
    item = (
        ItemBuilder()
        .backstage_pass()
        .with_quality(10)
        .with_sell_in(10)
        .build()
    )
    inn = GildedRoseInn(Inventory([item]))

    inn.update_inventory()

    assert item.quality == 12


def test_backstage_pass_quality_increases_by_three_when_concert_is_five_days_or_fewer_away():
    item = (
        ItemBuilder()
        .backstage_pass()
        .with_quality(10)
        .with_sell_in(5)
        .build()
    )
    inn = GildedRoseInn(Inventory([item]))

    inn.update_inventory()

    assert item.quality == 13


def test_backstage_pass_quality_drops_to_zero_after_the_concert():
    item = (
        ItemBuilder()
        .backstage_pass()
        .with_quality(20)
        .with_sell_in(0)
        .build()
    )
    inn = GildedRoseInn(Inventory([item]))

    inn.update_inventory()

    assert item.quality == 0


def test_backstage_pass_quality_never_exceeds_fifty_before_the_concert():
    item = (
        ItemBuilder()
        .backstage_pass()
        .with_quality(49)
        .with_sell_in(5)
        .build()
    )
    inn = GildedRoseInn(Inventory([item]))

    inn.update_inventory()

    assert item.quality == 50
