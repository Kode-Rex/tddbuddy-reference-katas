import { describe, it, expect } from 'vitest';
import {
  NumberOutOfRangeError,
  WinPattern,
} from '../src/card.js';
import { CardBuilder } from './cardBuilder.js';

// A complete 5x5 card used by scenarios that need every cell populated.
// Column ranges are honoured for realism even though the builder does
// not enforce them: B 1-15, I 16-30, N 31-45 (free at (2,2)), G 46-60, O 61-75.
const aFullCard = () =>
  new CardBuilder()
    .withNumberAt(0, 0, 3).withNumberAt(0, 1, 17).withNumberAt(0, 2, 33).withNumberAt(0, 3, 48).withNumberAt(0, 4, 62)
    .withNumberAt(1, 0, 8).withNumberAt(1, 1, 22).withNumberAt(1, 2, 38).withNumberAt(1, 3, 52).withNumberAt(1, 4, 67)
    .withNumberAt(2, 0, 11).withNumberAt(2, 1, 27)                        .withNumberAt(2, 3, 55).withNumberAt(2, 4, 70)
    .withNumberAt(3, 0, 4).withNumberAt(3, 1, 19).withNumberAt(3, 2, 41).withNumberAt(3, 3, 58).withNumberAt(3, 4, 73)
    .withNumberAt(4, 0, 15).withNumberAt(4, 1, 30).withNumberAt(4, 2, 45).withNumberAt(4, 3, 60).withNumberAt(4, 4, 75);

describe('Card', () => {
  it('blank card reports no win and no marks', () => {
    const card = aFullCard().build();

    expect(card.hasWon()).toBe(false);
    expect(card.winningPattern()).toEqual(WinPattern.none);
    for (let r = 0; r < 5; r++) {
      for (let c = 0; c < 5; c++) {
        if (!(r === 2 && c === 2)) expect(card.isMarked(r, c)).toBe(false);
      }
    }
  });

  it('free space starts marked', () => {
    const card = aFullCard().build();
    expect(card.isMarked(2, 2)).toBe(true);
  });

  it('marking a number that is on the card marks the matching cell', () => {
    const card = aFullCard().build();

    card.mark(3);

    expect(card.isMarked(0, 0)).toBe(true);
    expect(card.isMarked(0, 1)).toBe(false);
  });

  it('marking a number not on the card is a silent no-op', () => {
    const card = aFullCard().build();

    expect(() => card.mark(42)).not.toThrow();

    for (let r = 0; r < 5; r++) {
      for (let c = 0; c < 5; c++) {
        if (!(r === 2 && c === 2)) expect(card.isMarked(r, c)).toBe(false);
      }
    }
  });

  it('marking a number outside 1 to 75 raises', () => {
    const card = aFullCard().build();

    expect(() => card.mark(0)).toThrow(NumberOutOfRangeError);
    expect(() => card.mark(0)).toThrow('called number must be between 1 and 75');
    expect(() => card.mark(76)).toThrow(NumberOutOfRangeError);
    expect(() => card.mark(76)).toThrow('called number must be between 1 and 75');
  });

  it('completing row 0 wins on that row', () => {
    const card = aFullCard().build();

    [3, 17, 33, 48, 62].forEach((n) => card.mark(n));

    expect(card.hasWon()).toBe(true);
    expect(card.winningPattern()).toEqual(WinPattern.row(0));
  });

  it('completing column 4 wins on that column', () => {
    const card = aFullCard().build();

    [62, 67, 70, 73, 75].forEach((n) => card.mark(n));

    expect(card.winningPattern()).toEqual(WinPattern.column(4));
  });

  it('completing the main diagonal wins on DiagonalMain', () => {
    const card = aFullCard().build();

    [3, 22, 58, 75].forEach((n) => card.mark(n)); // (2,2) is the free space

    expect(card.winningPattern()).toEqual(WinPattern.diagonalMain);
  });

  it('completing the anti-diagonal wins on DiagonalAnti', () => {
    const card = aFullCard().build();

    [62, 52, 19, 15].forEach((n) => card.mark(n)); // (2,2) is the free space

    expect(card.winningPattern()).toEqual(WinPattern.diagonalAnti);
  });

  it('four marks in a row is not a win', () => {
    const card = aFullCard().build();

    [3, 17, 33, 48].forEach((n) => card.mark(n));

    expect(card.hasWon()).toBe(false);
    expect(card.winningPattern()).toEqual(WinPattern.none);
  });

  it('winning pattern scan order is rows then columns then diagonals', () => {
    const card = new CardBuilder()
      .withNumberAt(0, 0, 3).withNumberAt(0, 1, 17).withNumberAt(0, 2, 33).withNumberAt(0, 3, 48).withNumberAt(0, 4, 62)
      .withNumberAt(1, 0, 8)
      .withNumberAt(2, 0, 11)
      .withNumberAt(3, 0, 4)
      .withNumberAt(4, 0, 15)
      .withMarkAt(0, 0).withMarkAt(0, 1).withMarkAt(0, 2).withMarkAt(0, 3).withMarkAt(0, 4)
      .withMarkAt(1, 0).withMarkAt(2, 0).withMarkAt(3, 0).withMarkAt(4, 0)
      .build();

    expect(card.winningPattern()).toEqual(WinPattern.row(0));
  });

  it('CardBuilder produces the card the test literal describes', () => {
    const card = new CardBuilder().withNumberAt(0, 0, 3).build();

    expect(card.numberAt(0, 0)).toBe(3);
    expect(card.numberAt(2, 2)).toBeNull();
    expect(card.isMarked(2, 2)).toBe(true);
    expect(card.isMarked(0, 0)).toBe(false);

    card.mark(3);
    expect(card.isMarked(0, 0)).toBe(true);
  });
});
