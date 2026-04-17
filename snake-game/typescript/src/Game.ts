import type { Position } from './Position.js';
import { positionsEqual } from './Position.js';
import type { Direction } from './Direction.js';
import { move } from './Direction.js';
import { Snake } from './Snake.js';

export type GameState = 'Playing' | 'GameOver' | 'Won';
export type FoodSpawner = (emptyCells: Position[]) => Position;

/**
 * Aggregate: board + snake + food + score + state.
 * `tick()` advances the game by one step.
 */
export class Game {
  private readonly width: number;
  private readonly height: number;
  private readonly foodSpawner: FoodSpawner;

  snake: Snake;
  food: Position;
  score: number;
  state: GameState;

  constructor(
    width: number,
    height: number,
    foodSpawner: FoodSpawner,
    snake?: Snake,
    food?: Position,
    score?: number,
    state?: GameState,
  ) {
    this.width = width;
    this.height = height;
    this.foodSpawner = foodSpawner;
    this.snake = snake ?? new Snake([{ x: 0, y: 0 }], 'Right');
    this.score = score ?? 0;
    this.state = state ?? 'Playing';
    this.food = food ?? foodSpawner(this.emptyCells());
  }

  changeDirection(newDirection: Direction): void {
    if (this.state !== 'Playing') return;
    this.snake = this.snake.changeDirection(newDirection);
  }

  tick(): void {
    if (this.state !== 'Playing') return;

    const newHead = move(this.snake.direction, this.snake.head);

    if (this.isOutOfBounds(newHead) || this.isBodyCollision(newHead)) {
      this.state = 'GameOver';
      return;
    }

    const eatsFood = positionsEqual(newHead, this.food);
    this.snake = this.snake.move(eatsFood);

    if (eatsFood) {
      this.score++;

      if (this.snake.length === this.width * this.height) {
        this.state = 'Won';
        return;
      }

      this.food = this.foodSpawner(this.emptyCells());
    }
  }

  private isOutOfBounds(position: Position): boolean {
    return (
      position.x < 0 ||
      position.x >= this.width ||
      position.y < 0 ||
      position.y >= this.height
    );
  }

  private isBodyCollision(position: Position): boolean {
    return this.snake.occupiesPosition(position);
  }

  private emptyCells(): Position[] {
    const empty: Position[] = [];
    for (let x = 0; x < this.width; x++) {
      for (let y = 0; y < this.height; y++) {
        const pos: Position = { x, y };
        if (!this.snake.occupiesPosition(pos)) {
          empty.push(pos);
        }
      }
    }
    return empty;
  }
}
