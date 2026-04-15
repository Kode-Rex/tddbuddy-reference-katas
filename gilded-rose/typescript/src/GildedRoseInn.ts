import type { Inventory } from './Inventory.js';

export class GildedRoseInn {
  constructor(public readonly inventory: Inventory) {}

  updateInventory(): void {
    for (const item of this.inventory.items) {
      item.quality -= 1;
    }
  }
}
