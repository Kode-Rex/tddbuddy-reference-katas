import { describe, expect, it } from 'vitest';
import { GildedRoseInn } from '../src/GildedRoseInn.js';
import { Inventory } from '../src/Inventory.js';
import { ItemBuilder } from './ItemBuilder.js';

describe('Conjured items', () => {
  it('lose two quality per day while fresh', () => {
    const item = new ItemBuilder()
        .conjured()
        .withQuality(10)
        .withSellIn(5)
        .build();
    const inn = new GildedRoseInn(new Inventory([item]));

    inn.updateInventory();

    expect(item.quality).toBe(8);
  });

  it('lose four quality per day after the sell-by date', () => {
    const item = new ItemBuilder()
        .conjured()
        .withQuality(10)
        .withSellIn(0)
        .build();
    const inn = new GildedRoseInn(new Inventory([item]));

    inn.updateInventory();

    expect(item.quality).toBe(6);
  });

  it('quality never goes below zero', () => {
    const item = new ItemBuilder()
        .conjured()
        .withQuality(1)
        .withSellIn(5)
        .build();
    const inn = new GildedRoseInn(new Inventory([item]));

    inn.updateInventory();

    expect(item.quality).toBe(0);
  });
});
