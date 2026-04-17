import { Money } from '../src/Money.js';
import type { PartOption } from '../src/PartOption.js';
import type { PartType } from '../src/PartType.js';
import { FakePartSupplier } from './FakePartSupplier.js';

interface PartEntry {
  type: PartType;
  option: PartOption;
  price: number;
}

export class SupplierBuilder {
  private _name = 'Supplier';
  private readonly parts: PartEntry[] = [];

  named(name: string): this { this._name = name; return this; }

  withPart(type: PartType, option: PartOption, price: number): this {
    this.parts.push({ type, option, price });
    return this;
  }

  build(): FakePartSupplier {
    const supplier = new FakePartSupplier(this._name);
    for (const { type, option, price } of this.parts) {
      supplier.withPart(type, option, new Money(price));
    }
    return supplier;
  }
}
