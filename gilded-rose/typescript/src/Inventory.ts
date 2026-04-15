import type { Item } from './Item.js';

export class Inventory {
  constructor(public readonly items: readonly Item[]) {}
}
