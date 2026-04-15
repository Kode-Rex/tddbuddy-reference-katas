import { describe, expect, it } from 'vitest';
import { GildedRoseInn } from '../src/GildedRoseInn.js';
import { Inventory } from '../src/Inventory.js';
import { ItemBuilder } from './ItemBuilder.js';

describe('Backstage passes', () => {
  it('quality increases by one when concert is more than ten days away', () => {
    const item = new ItemBuilder()
        .backstagePass()
        .withQuality(10)
        .withSellIn(15)
        .build();
    const inn = new GildedRoseInn(new Inventory([item]));

    inn.updateInventory();

    expect(item.quality).toBe(11);
  });

  it('quality increases by two when concert is ten days or fewer away', () => {
    const item = new ItemBuilder()
        .backstagePass()
        .withQuality(10)
        .withSellIn(10)
        .build();
    const inn = new GildedRoseInn(new Inventory([item]));

    inn.updateInventory();

    expect(item.quality).toBe(12);
  });

  it('quality increases by three when concert is five days or fewer away', () => {
    const item = new ItemBuilder()
        .backstagePass()
        .withQuality(10)
        .withSellIn(5)
        .build();
    const inn = new GildedRoseInn(new Inventory([item]));

    inn.updateInventory();

    expect(item.quality).toBe(13);
  });

  it('quality drops to zero after the concert', () => {
    const item = new ItemBuilder()
        .backstagePass()
        .withQuality(20)
        .withSellIn(0)
        .build();
    const inn = new GildedRoseInn(new Inventory([item]));

    inn.updateInventory();

    expect(item.quality).toBe(0);
  });

  it('quality never exceeds fifty before the concert', () => {
    const item = new ItemBuilder()
        .backstagePass()
        .withQuality(49)
        .withSellIn(5)
        .build();
    const inn = new GildedRoseInn(new Inventory([item]));

    inn.updateInventory();

    expect(item.quality).toBe(50);
  });
});
