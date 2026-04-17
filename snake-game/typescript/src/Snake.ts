import type { Position } from './Position.js';
import { positionsEqual } from './Position.js';
import { type Direction, isOpposite, move } from './Direction.js';

/**
 * Ordered list of positions, head first. Immutable — movement returns a new Snake.
 */
export class Snake {
  readonly body: readonly Position[];
  readonly direction: Direction;

  constructor(body: readonly Position[], direction: Direction) {
    this.body = body;
    this.direction = direction;
  }

  get head(): Position {
    return this.body[0]!;
  }

  get length(): number {
    return this.body.length;
  }

  occupiesPosition(position: Position): boolean {
    return this.body.some((p) => positionsEqual(p, position));
  }

  move(grow: boolean): Snake {
    const newHead = move(this.direction, this.head);
    const newBody = [newHead, ...this.body];

    if (!grow) {
      newBody.pop();
    }

    return new Snake(newBody, this.direction);
  }

  changeDirection(newDirection: Direction): Snake {
    if (isOpposite(this.direction, newDirection)) {
      return this;
    }
    return new Snake(this.body, newDirection);
  }
}
