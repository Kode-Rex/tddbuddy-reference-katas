import type { Category } from '../src/Category.js';
import { Item } from '../src/Item.js';

export class ItemBuilder {
  private _name = 'Elixir of the Mongoose';
  private _category: Category = 'standard';
  private _quality = 10;
  private _sellIn = 5;

  standard(): this { this._category = 'standard'; return this; }
  aged(): this { this._category = 'aged'; return this; }
  legendary(): this { this._category = 'legendary'; return this; }
  backstagePass(): this { this._category = 'backstagePass'; return this; }
  conjured(): this { this._category = 'conjured'; return this; }

  named(name: string): this { this._name = name; return this; }
  withQuality(quality: number): this { this._quality = quality; return this; }
  withSellIn(sellIn: number): this { this._sellIn = sellIn; return this; }

  build(): Item {
    return new Item(this._name, this._category, this._quality, this._sellIn);
  }
}
