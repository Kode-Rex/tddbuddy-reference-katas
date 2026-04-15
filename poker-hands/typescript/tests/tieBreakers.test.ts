import { describe, it, expect } from 'vitest';
import { HandBuilder } from './HandBuilder.js';
import { Compare } from '../src/Compare.js';

describe('Comparing hands with same rank, different kickers', () => {
  it('higher high card wins when neither hand has a ranked combination', () => {
    const aceHigh = HandBuilder.fromString('2C 3H 4S 8C AH');
    const kingHigh = HandBuilder.fromString('2H 3D 5S 9C KD');
    expect(aceHigh.compareTo(kingHigh)).toBe(Compare.Player1Wins);
  });

  it('higher pair wins when both hands have a pair', () => {
    const kingsPair = HandBuilder.fromString('KH KD 5C 9S 3D');
    const twosPair = HandBuilder.fromString('2H 2D 5C 9S AD');
    expect(kingsPair.compareTo(twosPair)).toBe(Compare.Player1Wins);
  });

  it('higher kicker wins when both hands have the same pair', () => {
    const sevensWithAceKicker = HandBuilder.fromString('7H 7D AC 4S 2D');
    const sevensWithKingKicker = HandBuilder.fromString('7C 7S KH 4D 2H');
    expect(sevensWithAceKicker.compareTo(sevensWithKingKicker)).toBe(Compare.Player1Wins);
  });

  it('higher of two pairs wins when both hands have two pair with the same lower pair', () => {
    const acesAndTwos = HandBuilder.fromString('AH AD 2C 2S KD');
    const kingsAndTwos = HandBuilder.fromString('KH KC 2H 2D QS');
    expect(acesAndTwos.compareTo(kingsAndTwos)).toBe(Compare.Player1Wins);
  });
});
