import type { ComboDeal } from './ComboDeal.js';
import { Money } from './Money.js';
import { Product } from './Product.js';
import { Weight } from './Weight.js';

export class Checkout {
  private readonly products = new Map<string, Product>();
  private readonly quantities = new Map<string, number>();
  private readonly weights = new Map<string, Weight>();
  private readonly comboDeals: ComboDeal[];

  constructor(comboDeals: ComboDeal[] = []) {
    this.comboDeals = [...comboDeals];
  }

  scan(product: Product): void {
    this.ensureRegistered(product);
    this.quantities.set(product.sku, (this.quantities.get(product.sku) ?? 0) + 1);
  }

  scanWeighted(product: Product, weight: Weight): void {
    this.ensureRegistered(product);
    const existing = this.weights.get(product.sku) ?? Weight.zero;
    this.weights.set(product.sku, new Weight(existing.kg + weight.kg));
  }

  total(): Money {
    let result = Money.zero;

    const comboConsumed = new Map<string, number>();

    for (const deal of this.comboDeals) {
      const countA = (this.quantities.get(deal.skuA) ?? 0) - (comboConsumed.get(deal.skuA) ?? 0);
      const countB = (this.quantities.get(deal.skuB) ?? 0) - (comboConsumed.get(deal.skuB) ?? 0);
      const pairs = Math.min(countA, countB);

      if (pairs > 0) {
        result = result.plus(deal.dealPrice.times(pairs));
        comboConsumed.set(deal.skuA, (comboConsumed.get(deal.skuA) ?? 0) + pairs);
        comboConsumed.set(deal.skuB, (comboConsumed.get(deal.skuB) ?? 0) + pairs);
      }
    }

    for (const [sku, product] of this.products) {
      const quantity = (this.quantities.get(sku) ?? 0) - (comboConsumed.get(sku) ?? 0);
      const weight = this.weights.get(sku) ?? Weight.zero;

      if (quantity > 0 || weight.kg > 0) {
        result = result.plus(product.pricingRule.calculate(quantity, weight));
      }
    }

    return result;
  }

  private ensureRegistered(product: Product): void {
    if (!this.products.has(product.sku)) {
      this.products.set(product.sku, product);
    }
  }
}
