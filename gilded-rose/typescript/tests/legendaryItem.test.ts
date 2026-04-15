import { describe, expect, it } from 'vitest';
import { GildedRoseInn } from '../src/GildedRoseInn.js';
import { Inventory } from '../src/Inventory.js';
import { ItemBuilder } from './ItemBuilder.js';

describe('Legendary items', () => {
  it('never lose quality', () => {
    const item = new ItemBuilder().legendary().named('Sulfuras, Hand of Ragnaros').withQuality(80).withSellIn(5).build();
    const inn = new GildedRoseInn(new Inventory([item]));

    inn.updateInventory();

    expect(item.quality).toBe(80);
  });
});
