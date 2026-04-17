import type { Money } from './Money.js';
import type { PartOption } from './PartOption.js';
import type { PartType } from './PartType.js';

export interface PartQuote {
  readonly type: PartType;
  readonly option: PartOption;
  readonly price: Money;
  readonly supplierName: string;
}
