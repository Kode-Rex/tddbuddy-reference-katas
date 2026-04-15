import {
  CARD_SIZE,
  Card,
  FREE_COLUMN,
  FREE_ROW,
} from '../src/card.js';

// Test-folder synthesiser. Places specific numbers at specific coordinates
// without enforcing the B/I/N/G/O column ranges — tests set up the card
// state they need, not the card a real generator would produce.
export class CardBuilder {
  private readonly numbers: (number | null)[][] = Array.from(
    { length: CARD_SIZE },
    () => Array.from({ length: CARD_SIZE }, () => null as number | null),
  );
  private readonly marks: boolean[][] = Array.from(
    { length: CARD_SIZE },
    () => Array.from({ length: CARD_SIZE }, () => false),
  );

  constructor() {
    // Free space is blank and pre-marked on every card.
    this.marks[FREE_ROW]![FREE_COLUMN] = true;
  }

  withNumberAt(row: number, col: number, number: number): this {
    this.numbers[row]![col] = number;
    return this;
  }

  withMarkAt(row: number, col: number): this {
    this.marks[row]![col] = true;
    return this;
  }

  build(): Card {
    return new Card({ numbers: this.numbers, marks: this.marks });
  }
}
