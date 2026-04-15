import type { Inventory } from './Inventory.js';

export class GildedRoseInn {
  constructor(public readonly inventory: Inventory) {}

  updateInventory(): void {
    for (const item of this.inventory.items) {
      if (item.category === 'legendary') {
        continue;
      }
      if (item.category === 'aged') {
        const gain = item.sellIn <= 0 ? 2 : 1;
        item.quality = Math.min(50, item.quality + gain);
      } else if (item.category === 'backstagePass') {
        if (item.sellIn <= 0) {
          item.quality = 0;
        } else {
          const gain = item.sellIn <= 5 ? 3
            : item.sellIn <= 10 ? 2
            : 1;
          item.quality = Math.min(50, item.quality + gain);
        }
      } else {
        const degrade = item.sellIn <= 0 ? 2 : 1;
        item.quality = Math.max(0, item.quality - degrade);
      }
      item.sellIn -= 1;
    }
  }
}
