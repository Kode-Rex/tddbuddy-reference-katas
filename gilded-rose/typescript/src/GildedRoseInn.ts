import type { Inventory } from './Inventory.js';

export class GildedRoseInn {
  constructor(public readonly inventory: Inventory) {}

  updateInventory(): void {
    for (const item of this.inventory.items) {
      const degrade = item.sellIn <= 0 ? 2 : 1;
      item.quality -= degrade;
    }
  }
}
