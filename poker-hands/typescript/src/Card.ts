import { Rank } from './Rank.js';
import { Suit } from './Suit.js';

export interface Card {
  readonly rank: Rank;
  readonly suit: Suit;
}

export const card = (rank: Rank, suit: Suit): Card => ({ rank, suit });
