import { AccountLength, DigitHeight } from '../src/constants.js';
import { DigitBuilder } from './DigitBuilder.js';

export class AccountNumberBuilder {
  private digits: string[][] = Array.from({ length: AccountLength }, () =>
    new DigitBuilder().forDigit(0).build(),
  );

  fromString(digits: string): this {
    if (digits.length !== AccountLength) {
      throw new Error(`Expected ${AccountLength} digits, got ${digits.length}.`);
    }
    this.digits = Array.from(digits).map((ch) =>
      new DigitBuilder().forDigit(Number.parseInt(ch, 10)).build(),
    );
    return this;
  }

  withDigitAt(position: number, glyph: string[]): this {
    this.digits[position] = [...glyph];
    return this;
  }

  buildRows(): string[] {
    const rows: string[] = [];
    for (let r = 0; r < DigitHeight; r++) {
      rows.push(this.digits.map((d) => d[r]!).join(''));
    }
    return rows;
  }
}
