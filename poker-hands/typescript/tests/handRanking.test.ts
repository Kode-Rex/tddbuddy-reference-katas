import { describe, it, expect } from 'vitest';
import { HandBuilder } from './HandBuilder.js';
import { HandRank } from '../src/HandRank.js';

describe('Ranking a single hand', () => {
  it('hand of five unrelated cards is ranked as high card', () => {
    expect(HandBuilder.fromString('2H 5D 7C 9S KD').evaluate()).toBe(HandRank.HighCard);
  });

  it('hand with two cards of the same rank is ranked as a pair', () => {
    expect(HandBuilder.fromString('2H 2D 5C 9S KD').evaluate()).toBe(HandRank.Pair);
  });

  it('hand with two different pairs is ranked as two pair', () => {
    expect(HandBuilder.fromString('2H 2D 5C 5S KD').evaluate()).toBe(HandRank.TwoPair);
  });

  it('hand with three cards of the same rank is ranked as three of a kind', () => {
    expect(HandBuilder.fromString('2H 2D 2C 5S KD').evaluate()).toBe(HandRank.ThreeOfAKind);
  });

  it('hand of five consecutive ranks is ranked as a straight', () => {
    expect(HandBuilder.fromString('2H 3D 4C 5S 6D').evaluate()).toBe(HandRank.Straight);
  });

  it('hand of five cards of the same suit is ranked as a flush', () => {
    expect(HandBuilder.fromString('2H 5H 7H 9H KH').evaluate()).toBe(HandRank.Flush);
  });

  it('hand with a triple and a pair is ranked as a full house', () => {
    expect(HandBuilder.fromString('2H 2D 2C 5S 5D').evaluate()).toBe(HandRank.FullHouse);
  });

  it('hand with four cards of the same rank is ranked as four of a kind', () => {
    expect(HandBuilder.fromString('2H 2D 2C 2S KD').evaluate()).toBe(HandRank.FourOfAKind);
  });

  it('hand of five consecutive ranks in one suit is ranked as a straight flush', () => {
    expect(HandBuilder.fromString('2H 3H 4H 5H 6H').evaluate()).toBe(HandRank.StraightFlush);
  });
});
