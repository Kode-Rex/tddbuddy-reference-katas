import { describe, it, expect } from 'vitest';
import { BoardBuilder } from './BoardBuilder.js';
import { SnakeBuilder } from './SnakeBuilder.js';

describe('Game Over Behavior', () => {
  it('tick has no effect after game over', () => {
    const game = new BoardBuilder()
      .withSize(5, 5)
      .withSnake(new SnakeBuilder().at(4, 0).movingRight().build())
      .build();

    game.tick(); // game over — hit right wall
    expect(game.state).toBe('GameOver');

    const headAfterGameOver = { ...game.snake.head };
    const scoreAfterGameOver = game.score;

    game.tick(); // should have no effect

    expect(game.snake.head).toEqual(headAfterGameOver);
    expect(game.score).toBe(scoreAfterGameOver);
    expect(game.state).toBe('GameOver');
  });
});
