import { describe, it, expect } from 'vitest';
import { BoardBuilder } from './BoardBuilder.js';
import { SnakeBuilder } from './SnakeBuilder.js';

describe('Winning', () => {
  it('game is won when the snake fills the entire board', () => {
    const game = new BoardBuilder()
      .withSize(2, 1)
      .withSnake(new SnakeBuilder().at(0, 0).movingRight().build())
      .withFoodAt(1, 0)
      .build();

    game.tick();

    expect(game.state).toBe('Won');
    expect(game.snake.length).toBe(2);
  });
});
