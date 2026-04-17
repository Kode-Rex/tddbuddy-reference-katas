import type { Money } from './Money.js';
import type { PartQuote } from './PartQuote.js';

export interface CostBreakdown {
  readonly parts: readonly PartQuote[];
  readonly total: Money;
}
