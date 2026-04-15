from gilded_rose import GildedRoseInn, Inventory

from .item_builder import ItemBuilder


def test_mixed_inventory_each_item_follows_its_own_category_rules_on_the_same_day():
    standard = (
        ItemBuilder()
        .standard()
        .named("Elixir of the Mongoose")
        .with_quality(10)
        .with_sell_in(5)
        .build()
    )
    aged = (
        ItemBuilder()
        .aged()
        .named("Aged Brie")
        .with_quality(10)
        .with_sell_in(5)
        .build()
    )
    legendary = (
        ItemBuilder()
        .legendary()
        .named("Sulfuras, Hand of Ragnaros")
        .with_quality(80)
        .with_sell_in(5)
        .build()
    )
    pass_ = (
        ItemBuilder()
        .backstage_pass()
        .named("Backstage passes to a TAFKAL80ETC concert")
        .with_quality(10)
        .with_sell_in(7)
        .build()
    )
    conjured = (
        ItemBuilder()
        .conjured()
        .named("Conjured Mana Cake")
        .with_quality(10)
        .with_sell_in(5)
        .build()
    )

    inn = GildedRoseInn(Inventory([standard, aged, legendary, pass_, conjured]))

    inn.update_inventory()

    assert standard.quality == 9
    assert aged.quality == 11
    assert legendary.quality == 80
    assert pass_.quality == 12
    assert conjured.quality == 8


def test_multi_day_aging_ten_days_of_updates_applied_in_sequence_produce_correct_quality_progression():
    item = (
        ItemBuilder()
        .standard()
        .with_quality(20)
        .with_sell_in(5)
        .build()
    )
    inn = GildedRoseInn(Inventory([item]))

    for _ in range(10):
        inn.update_inventory()

    assert item.quality == 5
    assert item.sell_in == -5
