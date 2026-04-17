import { Money } from '../src/Money.js';
import type { PricingRule } from '../src/PricingRule.js';
import { BuyOneGetOneFree, MultiBuy, UnitPrice, WeightedPrice } from '../src/PricingRule.js';
import { Product } from '../src/Product.js';

export class ProductBuilder {
  private _sku = 'X';
  private _name = 'Default Item';
  private _pricingRule: PricingRule = new UnitPrice(new Money(100));

  withSku(sku: string): this { this._sku = sku; return this; }
  named(name: string): this { this._name = name; return this; }
  withUnitPrice(cents: number): this { this._pricingRule = new UnitPrice(new Money(cents)); return this; }
  withMultiBuy(groupSize: number, groupCents: number, itemCents: number): this {
    this._pricingRule = new MultiBuy(groupSize, new Money(groupCents), new Money(itemCents));
    return this;
  }
  withBuyOneGetOneFree(itemCents: number): this {
    this._pricingRule = new BuyOneGetOneFree(new Money(itemCents));
    return this;
  }
  withWeightedPrice(centsPerKg: number): this {
    this._pricingRule = new WeightedPrice(centsPerKg);
    return this;
  }
  withPricingRule(rule: PricingRule): this { this._pricingRule = rule; return this; }

  build(): Product { return new Product(this._sku, this._name, this._pricingRule); }
}
