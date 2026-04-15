import { describe, expect, it } from 'vitest';
import { GildedRoseInn } from '../src/GildedRoseInn.js';
import { Inventory } from '../src/Inventory.js';
import { ItemBuilder } from './ItemBuilder.js';

describe('Multi-item worlds', () => {
  it('mixed inventory: each item follows its own category rules on the same day', () => {
    const standard = new ItemBuilder().standard().named('Elixir of the Mongoose').withQuality(10).withSellIn(5).build();
    const aged = new ItemBuilder().aged().named('Aged Brie').withQuality(10).withSellIn(5).build();
    const legendary = new ItemBuilder().legendary().named('Sulfuras, Hand of Ragnaros').withQuality(80).withSellIn(5).build();
    const pass = new ItemBuilder().backstagePass().named('Backstage passes to a TAFKAL80ETC concert').withQuality(10).withSellIn(7).build();
    const conjured = new ItemBuilder().conjured().named('Conjured Mana Cake').withQuality(10).withSellIn(5).build();

    const inn = new GildedRoseInn(new Inventory([standard, aged, legendary, pass, conjured]));

    inn.updateInventory();

    expect(standard.quality).toBe(9);
    expect(aged.quality).toBe(11);
    expect(legendary.quality).toBe(80);
    expect(pass.quality).toBe(12);
    expect(conjured.quality).toBe(8);
  });
});
