import { Card, card } from '../src/Card.js';
import { Rank } from '../src/Rank.js';
import { Suit } from '../src/Suit.js';

export class CardBuilder {
  private rank: Rank = Rank.Two;
  private suit: Suit = Suit.Clubs;

  ofRank(rank: Rank): this { this.rank = rank; return this; }
  ofSuit(suit: Suit): this { this.suit = suit; return this; }

  build(): Card { return card(this.rank, this.suit); }
}
