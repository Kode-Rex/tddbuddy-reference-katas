import { Card } from '../src/Card.js';
import { Hand } from '../src/Hand.js';

/**
 * Two-shape builder: fluent `.with(card)` for tests that assemble a hand card-by-card,
 * and the static `fromString("2H 3D 5S 9C KD")` shorthand (delegates to Hand.parse)
 * for hand-level evaluation tests.
 */
export class HandBuilder {
  private readonly cards: Card[] = [];

  with(card: Card): this { this.cards.push(card); return this; }

  build(): Hand { return new Hand(this.cards); }

  static fromString(shorthand: string): Hand { return Hand.parse(shorthand); }
}
