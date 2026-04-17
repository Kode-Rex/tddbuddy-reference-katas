import type { PartOption } from './PartOption.js';
import type { PartQuote } from './PartQuote.js';
import type { PartType } from './PartType.js';
import type { PurchasedPart } from './PurchasedPart.js';

export interface PartSupplier {
  readonly name: string;
  getQuote(type: PartType, option: PartOption): PartQuote | null;
  purchase(type: PartType, option: PartOption): PurchasedPart;
}
