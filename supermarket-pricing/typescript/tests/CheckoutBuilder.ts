import { Checkout } from '../src/Checkout.js';
import { ComboDeal } from '../src/ComboDeal.js';
import { Money } from '../src/Money.js';
import type { Product } from '../src/Product.js';
import { Weight } from '../src/Weight.js';

interface Scanned { product: Product; count: number; }
interface Weighed { product: Product; weight: Weight; }

export class CheckoutBuilder {
  private readonly _scanned: Scanned[] = [];
  private readonly _weighed: Weighed[] = [];
  private readonly _comboDeals: ComboDeal[] = [];

  withScanned(product: Product, count = 1): this {
    this._scanned.push({ product, count });
    return this;
  }

  withWeighed(product: Product, kg: number): this {
    this._weighed.push({ product, weight: new Weight(kg) });
    return this;
  }

  withComboDeal(skuA: string, skuB: string, dealCents: number): this {
    this._comboDeals.push(new ComboDeal(skuA, skuB, new Money(dealCents)));
    return this;
  }

  build(): Checkout {
    const checkout = new Checkout(this._comboDeals);
    for (const { product, count } of this._scanned) {
      for (let i = 0; i < count; i++) {
        checkout.scan(product);
      }
    }
    for (const { product, weight } of this._weighed) {
      checkout.scanWeighted(product, weight);
    }
    return checkout;
  }
}
