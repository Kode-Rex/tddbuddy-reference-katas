import { describe, it, expect } from 'vitest';
import { BoardBuilder } from './BoardBuilder.js';
import { SnakeBuilder } from './SnakeBuilder.js';

describe('Wall Collisions', () => {
  it('snake hitting the right wall causes game over', () => {
    const game = new BoardBuilder()
      .withSize(5, 5)
      .withSnake(new SnakeBuilder().at(4, 0).movingRight().build())
      .build();

    game.tick();

    expect(game.state).toBe('GameOver');
  });

  it('snake hitting the bottom wall causes game over', () => {
    const game = new BoardBuilder()
      .withSize(5, 5)
      .withSnake(new SnakeBuilder().at(0, 4).movingDown().build())
      .build();

    game.tick();

    expect(game.state).toBe('GameOver');
  });

  it('snake hitting the left wall causes game over', () => {
    const game = new BoardBuilder()
      .withSize(5, 5)
      .withSnake(new SnakeBuilder().at(0, 0).movingLeft().build())
      .build();

    game.tick();

    expect(game.state).toBe('GameOver');
  });

  it('snake hitting the top wall causes game over', () => {
    const game = new BoardBuilder()
      .withSize(5, 5)
      .withSnake(new SnakeBuilder().at(0, 0).movingUp().build())
      .build();

    game.tick();

    expect(game.state).toBe('GameOver');
  });
});
