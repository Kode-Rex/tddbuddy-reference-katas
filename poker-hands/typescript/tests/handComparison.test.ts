import { describe, it, expect } from 'vitest';
import { HandBuilder } from './HandBuilder.js';
import { Compare } from '../src/Compare.js';

describe('Comparing hands with different ranks', () => {
  it('pair beats high card', () => {
    const pair = HandBuilder.fromString('2H 2D 5C 9S KD');
    const highCard = HandBuilder.fromString('3H 5D 7C 9S AD');
    expect(pair.compareTo(highCard)).toBe(Compare.Player1Wins);
  });

  it('flush beats straight', () => {
    const flush = HandBuilder.fromString('2H 5H 7H 9H KH');
    const straight = HandBuilder.fromString('2H 3D 4C 5S 6D');
    expect(flush.compareTo(straight)).toBe(Compare.Player1Wins);
  });

  it('straight flush beats four of a kind', () => {
    const straightFlush = HandBuilder.fromString('2H 3H 4H 5H 6H');
    const fourOfAKind = HandBuilder.fromString('AH AD AC AS KD');
    expect(straightFlush.compareTo(fourOfAKind)).toBe(Compare.Player1Wins);
  });
});
