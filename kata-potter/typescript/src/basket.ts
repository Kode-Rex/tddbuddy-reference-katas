// Identical byte-for-byte across C#, TypeScript, and Python.
// The exception messages are the spec (see ../SCENARIOS.md).
export const BasketMessages = {
  bookOutOfRange: 'book id must be between 1 and 5',
} as const;

export const BASE_PRICE = 8.0;
export const MIN_BOOK_ID = 1;
export const MAX_BOOK_ID = 5;

// Discount multiplier by set size. Index 0 is unused; index 1..5 are the
// discount fractions (0 for a 1-set, 0.25 for a 5-set).
export const SET_DISCOUNT: readonly number[] = [
  0.0, // index 0 — unused
  0.0, // 1 book
  0.05, // 2 books
  0.1, // 3 books
  0.2, // 4 books
  0.25, // 5 books
];

export function priceOfSet(distinctBooks: number): number {
  const discount = SET_DISCOUNT[distinctBooks]!;
  return distinctBooks * BASE_PRICE * (1 - discount);
}

export class BookOutOfRangeError extends Error {
  constructor() {
    super(BasketMessages.bookOutOfRange);
    this.name = 'BookOutOfRangeError';
  }
}

export class Basket {
  private readonly counts: number[];

  /** @internal — test-folder builder hook. */
  constructor(counts: number[]) {
    this.counts = counts.slice();
  }

  price(): number {
    const sets = groupIntoSets(this.counts);
    adjustFivePlusThreeIntoTwoFours(sets);
    let total = 0;
    for (let k = 1; k <= MAX_BOOK_ID; k++) {
      total += sets[k]! * priceOfSet(k);
    }
    return total;
  }
}

// Greedy pass: repeatedly pull the largest possible set of distinct titles
// out of the remaining counts. Returns a histogram where `sets[k]` is the
// number of k-sized sets.
function groupIntoSets(counts: readonly number[]): number[] {
  const remaining = counts.slice();
  const sets = new Array<number>(MAX_BOOK_ID + 1).fill(0);
  while (true) {
    let distinct = 0;
    for (let id = MIN_BOOK_ID; id <= MAX_BOOK_ID; id++) {
      if (remaining[id]! > 0) distinct++;
    }
    if (distinct === 0) break;
    sets[distinct]!++;
    for (let id = MIN_BOOK_ID; id <= MAX_BOOK_ID; id++) {
      if (remaining[id]! > 0) remaining[id]!--;
    }
  }
  return sets;
}

// Adjustment pass: a 5-set plus a 3-set always costs more than two 4-sets
// (51.60 vs 51.20 per pairing). Swap (5,3) -> (4,4) as many times as both
// counts allow. This is the only local swap that improves on greedy for
// the standard five-title discount table.
function adjustFivePlusThreeIntoTwoFours(sets: number[]): void {
  const swaps = Math.min(sets[5]!, sets[3]!);
  sets[5]! -= swaps;
  sets[3]! -= swaps;
  sets[4]! += 2 * swaps;
}
