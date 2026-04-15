import { LineItemNotFoundError } from './errors.js';
import { LineItem } from './LineItem.js';
import { Money } from './Money.js';
import { Product } from './Product.js';
import { Quantity } from './Quantity.js';

const ONE = new Quantity(1);

export class Cart {
  private readonly _lines: LineItem[] = [];

  get lines(): readonly LineItem[] { return this._lines; }
  get isEmpty(): boolean { return this._lines.length === 0; }

  add(product: Product, quantity: Quantity = ONE): void {
    const existing = this.findLine(product.sku);
    if (existing === undefined) {
      this._lines.push(new LineItem(product, quantity));
    } else {
      existing.incrementBy(quantity);
    }
  }

  remove(sku: string): void {
    const index = this._lines.findIndex((l) => l.product.sku === sku);
    if (index >= 0) {
      this._lines.splice(index, 1);
    }
  }

  updateQuantity(sku: string, quantity: number): void {
    const line = this.findLine(sku);
    if (line === undefined) {
      throw new LineItemNotFoundError(`No line item for SKU '${sku}'`);
    }
    line.setQuantity(new Quantity(quantity));
  }

  total(): Money {
    return this._lines.reduce((sum, line) => sum.plus(line.subtotal()), Money.zero);
  }

  private findLine(sku: string): LineItem | undefined {
    return this._lines.find((l) => l.product.sku === sku);
  }
}
