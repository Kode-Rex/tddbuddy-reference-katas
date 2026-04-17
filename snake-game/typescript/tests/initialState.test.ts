import { describe, it, expect } from 'vitest';
import { BoardBuilder } from './BoardBuilder.js';

describe('Initial State', () => {
  it('a new game has the snake at (0, 0) moving right', () => {
    const game = new BoardBuilder().build();

    expect(game.snake.head).toEqual({ x: 0, y: 0 });
    expect(game.snake.direction).toBe('Right');
  });

  it('a new game has a score of zero', () => {
    const game = new BoardBuilder().build();

    expect(game.score).toBe(0);
  });

  it('a new game is in playing state', () => {
    const game = new BoardBuilder().build();

    expect(game.state).toBe('Playing');
  });

  it('a new game has food on the board', () => {
    const game = new BoardBuilder().withFoodAt(3, 3).build();

    expect(game.food).toEqual({ x: 3, y: 3 });
  });
});
