import { Card, card } from './Card.js';
import { Rank } from './Rank.js';
import { Suit } from './Suit.js';
import { HandRank } from './HandRank.js';
import { Compare } from './Compare.js';
import { InvalidHandError } from './InvalidHandError.js';

const HAND_SIZE = 5;

const RANK_CODES: Record<string, Rank> = {
  '2': Rank.Two, '3': Rank.Three, '4': Rank.Four, '5': Rank.Five,
  '6': Rank.Six, '7': Rank.Seven, '8': Rank.Eight, '9': Rank.Nine,
  T: Rank.Ten, J: Rank.Jack, Q: Rank.Queen, K: Rank.King, A: Rank.Ace,
};

const SUIT_CODES: Record<string, Suit> = {
  C: Suit.Clubs, D: Suit.Diamonds, H: Suit.Hearts, S: Suit.Spades,
};

export class Hand {
  readonly cards: readonly Card[];

  constructor(cards: readonly Card[]) {
    if (cards.length !== HAND_SIZE) {
      throw new InvalidHandError(`A hand must have exactly 5 cards (got ${cards.length})`);
    }
    this.cards = [...cards];
  }

  /**
   * Parse a hand from shorthand notation like "2H 3D 5S 9C KD".
   * Each token is a two-character card: rank code (2-9, T, J, Q, K, A) + suit code (C, D, H, S).
   */
  static parse(shorthand: string): Hand {
    const tokens = shorthand.trim().split(/\s+/).filter((t) => t.length > 0);
    const cards = tokens.map((token) => parseCard(token));
    return new Hand(cards);
  }

  evaluate(): HandRank {
    const groups = rankGroupsByCountDescending(this.cards);
    const counts = groups.map((g) => g.count);
    const isFlush = new Set(this.cards.map((c) => c.suit)).size === 1;
    const isStraight = computeIsStraight(this.cards);

    if (isStraight && isFlush) return HandRank.StraightFlush;
    if (counts[0] === 4) return HandRank.FourOfAKind;
    if (counts[0] === 3 && counts[1] === 2) return HandRank.FullHouse;
    if (isFlush) return HandRank.Flush;
    if (isStraight) return HandRank.Straight;
    if (counts[0] === 3) return HandRank.ThreeOfAKind;
    if (counts[0] === 2 && counts[1] === 2) return HandRank.TwoPair;
    if (counts[0] === 2) return HandRank.Pair;
    return HandRank.HighCard;
  }

  compareTo(other: Hand): Compare {
    const myRank = this.evaluate();
    const theirRank = other.evaluate();
    if (myRank > theirRank) return Compare.Player1Wins;
    if (myRank < theirRank) return Compare.Player2Wins;

    const mySig = tieBreakSignature(this.cards);
    const theirSig = tieBreakSignature(other.cards);
    for (let i = 0; i < mySig.length; i++) {
      const a = mySig[i]!;
      const b = theirSig[i]!;
      if (a > b) return Compare.Player1Wins;
      if (a < b) return Compare.Player2Wins;
    }
    return Compare.Tie;
  }
}

function parseCard(token: string): Card {
  if (token.length !== 2) {
    throw new InvalidHandError(`Invalid card token '${token}'`);
  }
  const rank = RANK_CODES[token[0]!];
  if (rank === undefined) throw new InvalidHandError(`Invalid rank code '${token[0]}'`);
  const suit = SUIT_CODES[token[1]!];
  if (suit === undefined) throw new InvalidHandError(`Invalid suit code '${token[1]}'`);
  return card(rank, suit);
}

interface RankGroup { rank: Rank; count: number; }

function rankGroupsByCountDescending(cards: readonly Card[]): RankGroup[] {
  const counts = new Map<Rank, number>();
  for (const c of cards) counts.set(c.rank, (counts.get(c.rank) ?? 0) + 1);
  return [...counts.entries()]
    .map(([rank, count]) => ({ rank, count }))
    .sort((a, b) => (b.count - a.count) || (b.rank - a.rank));
}

/**
 * Canonical tie-break signature: ranks ordered first by group size (descending),
 * then by rank (descending). A positional compare correctly implements every
 * same-rank tie-breaker rule in SCENARIOS.md.
 */
function tieBreakSignature(cards: readonly Card[]): Rank[] {
  const signature: Rank[] = [];
  for (const g of rankGroupsByCountDescending(cards)) {
    for (let i = 0; i < g.count; i++) signature.push(g.rank);
  }
  return signature;
}

function computeIsStraight(cards: readonly Card[]): boolean {
  const sorted = cards.map((c) => c.rank as number).sort((a, b) => a - b);
  for (let i = 1; i < sorted.length; i++) {
    if (sorted[i]! !== sorted[i - 1]! + 1) return false;
  }
  return true;
}
