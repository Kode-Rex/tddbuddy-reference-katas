import type { DiscountPolicy } from './DiscountPolicy.js';
import { NoDiscount } from './DiscountPolicy.js';
import { Money } from './Money.js';

export class Product {
  readonly discountPolicy: DiscountPolicy;

  constructor(
    public readonly sku: string,
    public readonly name: string,
    public readonly unitPrice: Money,
    discountPolicy?: DiscountPolicy,
  ) {
    if (sku.trim().length === 0) {
      throw new Error('SKU must not be empty');
    }
    this.discountPolicy = discountPolicy ?? NoDiscount.instance;
  }
}
