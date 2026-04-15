import { describe, it, expect } from 'vitest';
import { HandBuilder } from './HandBuilder.js';
import { Compare } from '../src/Compare.js';

describe('Ties', () => {
  it('two hands with identical ranks and kickers tie', () => {
    const player1 = HandBuilder.fromString('2H 3D 5S 9C KD');
    const player2 = HandBuilder.fromString('2D 3H 5C 9S KH');
    expect(player1.compareTo(player2)).toBe(Compare.Tie);
  });
});
