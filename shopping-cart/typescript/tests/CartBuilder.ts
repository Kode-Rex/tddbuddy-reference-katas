import { Cart } from '../src/Cart.js';
import type { Product } from '../src/Product.js';
import { Quantity } from '../src/Quantity.js';

interface Seed { product: Product; quantity: Quantity; }

export class CartBuilder {
  private readonly _seeded: Seed[] = [];

  withProduct(product: Product, quantity: Quantity = new Quantity(1)): this {
    this._seeded.push({ product, quantity });
    return this;
  }

  build(): Cart {
    const cart = new Cart();
    for (const { product, quantity } of this._seeded) {
      cart.add(product, quantity);
    }
    return cart;
  }
}
