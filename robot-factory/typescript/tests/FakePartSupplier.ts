import { Money } from '../src/Money.js';
import type { PartOption } from '../src/PartOption.js';
import type { PartQuote } from '../src/PartQuote.js';
import type { PartSupplier } from '../src/PartSupplier.js';
import type { PartType } from '../src/PartType.js';
import type { PurchasedPart } from '../src/PurchasedPart.js';

export class FakePartSupplier implements PartSupplier {
  private readonly catalog = new Map<string, Money>();
  private readonly _purchaseLog: PurchasedPart[] = [];

  constructor(public readonly name: string) {}

  get purchaseLog(): readonly PurchasedPart[] { return this._purchaseLog; }

  withPart(type: PartType, option: PartOption, price: Money): this {
    this.catalog.set(`${type}:${option}`, price);
    return this;
  }

  getQuote(type: PartType, option: PartOption): PartQuote | null {
    const price = this.catalog.get(`${type}:${option}`);
    return price ? { type, option, price, supplierName: this.name } : null;
  }

  purchase(type: PartType, option: PartOption): PurchasedPart {
    const price = this.catalog.get(`${type}:${option}`)!;
    const part: PurchasedPart = { type, option, price, supplierName: this.name };
    this._purchaseLog.push(part);
    return part;
  }
}
