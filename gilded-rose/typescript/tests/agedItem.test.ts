import { describe, expect, it } from 'vitest';
import { GildedRoseInn } from '../src/GildedRoseInn.js';
import { Inventory } from '../src/Inventory.js';
import { ItemBuilder } from './ItemBuilder.js';

describe('Aged items', () => {
  it('gain one quality per day while fresh', () => {
    const item = new ItemBuilder().aged().withQuality(10).withSellIn(5).build();
    const inn = new GildedRoseInn(new Inventory([item]));

    inn.updateInventory();

    expect(item.quality).toBe(11);
  });

  it('gain two quality per day after the sell-by date', () => {
    const item = new ItemBuilder().aged().withQuality(10).withSellIn(0).build();
    const inn = new GildedRoseInn(new Inventory([item]));

    inn.updateInventory();

    expect(item.quality).toBe(12);
  });
});
