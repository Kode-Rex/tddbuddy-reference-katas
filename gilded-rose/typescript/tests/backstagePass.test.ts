import { describe, expect, it } from 'vitest';
import { GildedRoseInn } from '../src/GildedRoseInn.js';
import { Inventory } from '../src/Inventory.js';
import { ItemBuilder } from './ItemBuilder.js';

describe('Backstage passes', () => {
  it('quality increases by one when concert is more than ten days away', () => {
    const item = new ItemBuilder().backstagePass().withQuality(10).withSellIn(15).build();
    const inn = new GildedRoseInn(new Inventory([item]));

    inn.updateInventory();

    expect(item.quality).toBe(11);
  });
});
