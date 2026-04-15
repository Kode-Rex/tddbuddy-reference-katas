import type { Category } from './Category.js';

export class Item {
  constructor(
    public readonly name: string,
    public readonly category: Category,
    public quality: number,
    public sellIn: number,
  ) {}
}
