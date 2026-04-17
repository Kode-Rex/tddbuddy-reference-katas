import type { Position } from './Position.js';

export type Direction = 'Up' | 'Down' | 'Left' | 'Right';

const opposites: Record<Direction, Direction> = {
  Up: 'Down',
  Down: 'Up',
  Left: 'Right',
  Right: 'Left',
};

export function isOpposite(a: Direction, b: Direction): boolean {
  return opposites[a] === b;
}

export function move(direction: Direction, position: Position): Position {
  switch (direction) {
    case 'Up':
      return { x: position.x, y: position.y - 1 };
    case 'Down':
      return { x: position.x, y: position.y + 1 };
    case 'Left':
      return { x: position.x - 1, y: position.y };
    case 'Right':
      return { x: position.x + 1, y: position.y };
  }
}
