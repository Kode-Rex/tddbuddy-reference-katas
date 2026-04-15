// Identical byte-for-byte across C#, TypeScript, and Python.
// The exception messages are the spec (see ../SCENARIOS.md).
export const CardMessages = {
  numberOutOfRange: 'called number must be between 1 and 75',
} as const;

export const CARD_SIZE = 5;
export const FREE_ROW = 2;
export const FREE_COLUMN = 2;
export const MIN_NUMBER = 1;
export const MAX_NUMBER = 75;

export class NumberOutOfRangeError extends Error {
  constructor() {
    super(CardMessages.numberOutOfRange);
    this.name = 'NumberOutOfRangeError';
  }
}

export type WinPattern =
  | { kind: 'None' }
  | { kind: 'Row'; index: number }
  | { kind: 'Column'; index: number }
  | { kind: 'DiagonalMain' }
  | { kind: 'DiagonalAnti' };

export const WinPattern = {
  none: { kind: 'None' } as WinPattern,
  row: (index: number): WinPattern => ({ kind: 'Row', index }),
  column: (index: number): WinPattern => ({ kind: 'Column', index }),
  diagonalMain: { kind: 'DiagonalMain' } as WinPattern,
  diagonalAnti: { kind: 'DiagonalAnti' } as WinPattern,
};

type NumberGrid = (number | null)[][];
type MarkGrid = boolean[][];

export interface CardState {
  readonly numbers: NumberGrid;
  readonly marks: MarkGrid;
}

export class Card {
  private readonly numbers: NumberGrid;
  private readonly marks: MarkGrid;

  /** @internal — test-folder builder hook; production code should construct via a generator. */
  constructor(state: CardState) {
    this.numbers = state.numbers.map((row) => row.slice());
    this.marks = state.marks.map((row) => row.slice());
  }

  numberAt(row: number, col: number): number | null {
    return this.numbers[row]![col]!;
  }

  isMarked(row: number, col: number): boolean {
    return this.marks[row]![col]!;
  }

  mark(number: number): void {
    if (number < MIN_NUMBER || number > MAX_NUMBER) {
      throw new NumberOutOfRangeError();
    }
    for (let r = 0; r < CARD_SIZE; r++) {
      for (let c = 0; c < CARD_SIZE; c++) {
        if (this.numbers[r]![c] === number) {
          this.marks[r]![c] = true;
        }
      }
    }
  }

  hasWon(): boolean {
    return this.winningPattern().kind !== 'None';
  }

  winningPattern(): WinPattern {
    for (let r = 0; r < CARD_SIZE; r++) {
      if (this.rowMarked(r)) return WinPattern.row(r);
    }
    for (let c = 0; c < CARD_SIZE; c++) {
      if (this.columnMarked(c)) return WinPattern.column(c);
    }
    if (this.mainDiagonalMarked()) return WinPattern.diagonalMain;
    if (this.antiDiagonalMarked()) return WinPattern.diagonalAnti;
    return WinPattern.none;
  }

  private rowMarked(r: number): boolean {
    for (let c = 0; c < CARD_SIZE; c++) if (!this.marks[r]![c]) return false;
    return true;
  }

  private columnMarked(c: number): boolean {
    for (let r = 0; r < CARD_SIZE; r++) if (!this.marks[r]![c]) return false;
    return true;
  }

  private mainDiagonalMarked(): boolean {
    for (let i = 0; i < CARD_SIZE; i++) if (!this.marks[i]![i]) return false;
    return true;
  }

  private antiDiagonalMarked(): boolean {
    for (let i = 0; i < CARD_SIZE; i++) if (!this.marks[i]![CARD_SIZE - 1 - i]) return false;
    return true;
  }
}
