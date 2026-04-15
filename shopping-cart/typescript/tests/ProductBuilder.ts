import type { DiscountPolicy } from '../src/DiscountPolicy.js';
import { Money } from '../src/Money.js';
import { Product } from '../src/Product.js';

export class ProductBuilder {
  private _sku = 'SKU-DEFAULT';
  private _name = 'Widget';
  private _unitPrice = new Money(10);
  private _discountPolicy?: DiscountPolicy;

  withSku(sku: string): this { this._sku = sku; return this; }
  named(name: string): this { this._name = name; return this; }
  pricedAt(amount: number): this { this._unitPrice = new Money(amount); return this; }
  withDiscount(policy: DiscountPolicy): this { this._discountPolicy = policy; return this; }

  build(): Product { return new Product(this._sku, this._name, this._unitPrice, this._discountPolicy); }
}
