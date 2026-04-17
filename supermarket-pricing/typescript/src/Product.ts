import type { PricingRule } from './PricingRule.js';

export class Product {
  constructor(
    public readonly sku: string,
    public readonly name: string,
    public readonly pricingRule: PricingRule,
  ) {
    if (sku.trim().length === 0) {
      throw new Error('SKU must not be empty');
    }
  }
}
