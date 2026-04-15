import { describe, it, expect } from 'vitest';
import { HandBuilder } from './HandBuilder.js';
import { CardBuilder } from './CardBuilder.js';
import { card } from '../src/Card.js';
import { Rank } from '../src/Rank.js';
import { Suit } from '../src/Suit.js';
import { InvalidHandError } from '../src/InvalidHandError.js';

describe('Hand construction', () => {
  it('a hand with five cards is valid', () => {
    const aceOfSpades = new CardBuilder().ofRank(Rank.Ace).ofSuit(Suit.Spades).build();
    const hand = new HandBuilder()
      .with(aceOfSpades)
      .with(card(Rank.King, Suit.Spades))
      .with(card(Rank.Queen, Suit.Spades))
      .with(card(Rank.Jack, Suit.Spades))
      .with(card(Rank.Ten, Suit.Spades))
      .build();

    expect(hand.cards).toHaveLength(5);
    expect(hand.cards[0]).toEqual(aceOfSpades);
  });

  it('a hand with fewer than five cards is rejected', () => {
    expect(() => HandBuilder.fromString('2H 3D 5S 9C'))
      .toThrow(new InvalidHandError('A hand must have exactly 5 cards (got 4)'));
  });

  it('a hand with more than five cards is rejected', () => {
    expect(() => HandBuilder.fromString('2H 3D 5S 9C KD AH'))
      .toThrow(new InvalidHandError('A hand must have exactly 5 cards (got 6)'));
  });
});
