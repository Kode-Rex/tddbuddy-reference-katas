import { describe, expect, it } from 'vitest';
import { GildedRoseInn } from '../src/GildedRoseInn.js';
import { Inventory } from '../src/Inventory.js';
import { ItemBuilder } from './ItemBuilder.js';

describe('Standard items', () => {
  it('lose one quality per day while fresh', () => {
    const item = new ItemBuilder().standard().withQuality(10).withSellIn(5).build();
    const inn = new GildedRoseInn(new Inventory([item]));

    inn.updateInventory();

    expect(item.quality).toBe(9);
  });

  it('lose two quality per day after the sell-by date', () => {
    const item = new ItemBuilder().standard().withQuality(10).withSellIn(0).build();
    const inn = new GildedRoseInn(new Inventory([item]));

    inn.updateInventory();

    expect(item.quality).toBe(8);
  });
});
