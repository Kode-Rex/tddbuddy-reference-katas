import { Money } from '../src/Money.js';
import { Product } from '../src/Product.js';
import { BakeSale } from '../src/BakeSale.js';

export class OrderBuilder {
  private readonly _inventory: Product[] = [];

  withProduct(product: Product): this {
    this._inventory.push(product);
    return this;
  }

  withDefaultInventory(): this {
    this._inventory.push(new Product('Brownie', new Money(0.75), 'B', 48));
    this._inventory.push(new Product('Muffin', new Money(1.00), 'M', 36));
    this._inventory.push(new Product('Cake Pop', new Money(1.35), 'C', 24));
    this._inventory.push(new Product('Water', new Money(1.50), 'W', 30));
    return this;
  }

  build(): BakeSale {
    return new BakeSale(this._inventory);
  }
}
