// Builds a 3x3 OCR digit grid. Use `forDigit` for canonical glyphs,
// `withRow` to override specific rows (useful for crafting unknown digits).
const Canonical: Record<number, readonly string[]> = {
  0: [' _ ', '| |', '|_|'],
  1: ['   ', '  |', '  |'],
  2: [' _ ', ' _|', '|_ '],
  3: [' _ ', ' _|', ' _|'],
  4: ['   ', '|_|', '  |'],
  5: [' _ ', '|_ ', ' _|'],
  6: [' _ ', '|_ ', '|_|'],
  7: [' _ ', '  |', '  |'],
  8: [' _ ', '|_|', '|_|'],
  9: [' _ ', '|_|', ' _|'],
};

export class DigitBuilder {
  private rows: string[] = ['   ', '   ', '   '];

  forDigit(value: number): this {
    this.rows = [...Canonical[value]!];
    return this;
  }

  withRow(index: number, row: string): this {
    this.rows[index] = row;
    return this;
  }

  build(): string[] {
    return [...this.rows];
  }
}
