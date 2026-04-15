import { describe, expect, it } from 'vitest';
import { Digit } from '../src/Digit.js';
import { InvalidAccountNumberFormatException } from '../src/InvalidAccountNumberFormatException.js';
import { parseAccountNumber, parseDigit } from '../src/Parser.js';
import { AccountNumberBuilder } from './AccountNumberBuilder.js';
import { DigitBuilder } from './DigitBuilder.js';

describe('Bank OCR', () => {
  // --- Digit Parsing ---------------------------------------------------

  for (const value of [0, 1, 2, 3, 4, 5, 6, 7, 8, 9]) {
    it(`parses the canonical glyph for ${value}`, () => {
      const glyph = new DigitBuilder().forDigit(value).build();
      expect(parseDigit(glyph).equals(Digit.of(value))).toBe(true);
    });
  }

  it('a non-canonical glyph parses as unknown', () => {
    const garbled = new DigitBuilder().forDigit(8).withRow(1, '|X|').build();
    expect(parseDigit(garbled).equals(Digit.unknown)).toBe(true);
  });

  // --- Account Number Parsing -----------------------------------------

  it('parses a full 9-digit account number from a 3x27 block', () => {
    const rows = new AccountNumberBuilder().fromString('123456789').buildRows();
    const account = parseAccountNumber(rows);
    expect(account.number).toBe('123456789');
    expect(account.isLegible).toBe(true);
  });

  it('an account with one unreadable digit parses with an unknown in that position', () => {
    const garbled = new DigitBuilder().forDigit(9).withRow(2, '|X|').build();
    const rows = new AccountNumberBuilder().fromString('123456789').withDigitAt(8, garbled).buildRows();
    const account = parseAccountNumber(rows);
    expect(account.number).toBe('12345678?');
    expect(account.isLegible).toBe(false);
  });

  it('rejects an OCR block with the wrong number of rows', () => {
    const twoRows = [' '.repeat(27), ' '.repeat(27)];
    expect(() => parseAccountNumber(twoRows)).toThrow(InvalidAccountNumberFormatException);
  });

  it('rejects an OCR block with the wrong row width', () => {
    const rows = [' '.repeat(26), ' '.repeat(27), ' '.repeat(27)];
    expect(() => parseAccountNumber(rows)).toThrow(InvalidAccountNumberFormatException);
  });

  // --- Checksum Validation ---------------------------------------------

  it('a legible account with a valid checksum reports as valid', () => {
    const rows = new AccountNumberBuilder().fromString('345882865').buildRows();
    expect(parseAccountNumber(rows).isChecksumValid).toBe(true);
  });

  it('a legible account with an invalid checksum reports as invalid', () => {
    const rows = new AccountNumberBuilder().fromString('345882866').buildRows();
    expect(parseAccountNumber(rows).isChecksumValid).toBe(false);
  });

  it('an account containing an unknown digit is not considered for checksum', () => {
    const garbled = new DigitBuilder().forDigit(5).withRow(0, 'X_X').build();
    const rows = new AccountNumberBuilder().fromString('345882865').withDigitAt(8, garbled).buildRows();
    expect(parseAccountNumber(rows).isChecksumValid).toBe(false);
  });

  // --- Status Reporting ------------------------------------------------

  it('status for a valid account is just the number', () => {
    const rows = new AccountNumberBuilder().fromString('345882865').buildRows();
    expect(parseAccountNumber(rows).status).toBe('345882865');
  });

  it('status for a bad-checksum account appends ERR', () => {
    const rows = new AccountNumberBuilder().fromString('345882866').buildRows();
    expect(parseAccountNumber(rows).status).toBe('345882866 ERR');
  });

  it('status for an illegible account appends ILL', () => {
    const garbled = new DigitBuilder().forDigit(5).withRow(0, 'X_X').build();
    const rows = new AccountNumberBuilder().fromString('345882865').withDigitAt(8, garbled).buildRows();
    expect(parseAccountNumber(rows).status).toBe('34588286? ILL');
  });

  // --- Builders --------------------------------------------------------

  it('AccountNumberBuilder.fromString renders a 3x27 OCR block matching the canonical glyphs', () => {
    const rows = new AccountNumberBuilder().fromString('123456789').buildRows();
    expect(rows).toHaveLength(3);
    expect(rows[0]).toBe('    _  _     _  _  _  _  _ ');
    expect(rows[1]).toBe('  | _| _||_||_ |_   ||_||_|');
    expect(rows[2]).toBe('  ||_  _|  | _||_|  ||_| _|');
  });
});
