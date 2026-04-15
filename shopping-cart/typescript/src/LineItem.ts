import { Money } from './Money.js';
import { Product } from './Product.js';
import { Quantity } from './Quantity.js';

export class LineItem {
  private _quantity: Quantity;

  constructor(public readonly product: Product, quantity: Quantity) {
    this._quantity = quantity;
  }

  get quantity(): Quantity { return this._quantity; }

  subtotal(): Money {
    return this.product.discountPolicy.apply(this.product.unitPrice, this._quantity);
  }

  incrementBy(delta: Quantity): void {
    this._quantity = this._quantity.plus(delta);
  }

  setQuantity(quantity: Quantity): void {
    this._quantity = quantity;
  }
}
