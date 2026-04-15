import { AccountNumber } from './AccountNumber.js';
import { AccountLength, DigitHeight, DigitWidth, RowWidth } from './constants.js';
import { Digit } from './Digit.js';
import { InvalidAccountNumberFormatException } from './InvalidAccountNumberFormatException.js';

// Canonical glyphs: each entry is the 9-char flattened 3x3 grid (top | middle | bottom).
const Glyphs: ReadonlyMap<string, number> = new Map<string, number>([
  [' _ ' + '| |' + '|_|', 0],
  ['   ' + '  |' + '  |', 1],
  [' _ ' + ' _|' + '|_ ', 2],
  [' _ ' + ' _|' + ' _|', 3],
  ['   ' + '|_|' + '  |', 4],
  [' _ ' + '|_ ' + ' _|', 5],
  [' _ ' + '|_ ' + '|_|', 6],
  [' _ ' + '  |' + '  |', 7],
  [' _ ' + '|_|' + '|_|', 8],
  [' _ ' + '|_|' + ' _|', 9],
]);

export function parseDigit(threeRowBlock: readonly string[]): Digit {
  if (threeRowBlock.length !== DigitHeight) {
    throw new InvalidAccountNumberFormatException(
      `Digit block must have exactly ${DigitHeight} rows.`,
    );
  }
  for (const row of threeRowBlock) {
    if (row.length !== DigitWidth) {
      throw new InvalidAccountNumberFormatException(
        `Digit block rows must be exactly ${DigitWidth} characters wide.`,
      );
    }
  }
  const key = threeRowBlock[0]! + threeRowBlock[1]! + threeRowBlock[2]!;
  const value = Glyphs.get(key);
  return value === undefined ? Digit.unknown : Digit.of(value);
}

export function parseAccountNumber(threeRows: readonly string[]): AccountNumber {
  if (threeRows.length !== DigitHeight) {
    throw new InvalidAccountNumberFormatException(
      `OCR block must have exactly ${DigitHeight} rows.`,
    );
  }
  for (const row of threeRows) {
    if (row.length !== RowWidth) {
      throw new InvalidAccountNumberFormatException(
        `OCR block rows must be exactly ${RowWidth} characters wide.`,
      );
    }
  }
  const digits: Digit[] = [];
  for (let i = 0; i < AccountLength; i++) {
    const start = i * DigitWidth;
    const block = [
      threeRows[0]!.substring(start, start + DigitWidth),
      threeRows[1]!.substring(start, start + DigitWidth),
      threeRows[2]!.substring(start, start + DigitWidth),
    ];
    digits.push(parseDigit(block));
  }
  return new AccountNumber(digits);
}
