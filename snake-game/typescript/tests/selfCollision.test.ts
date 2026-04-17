import { describe, it, expect } from 'vitest';
import { BoardBuilder } from './BoardBuilder.js';
import { SnakeBuilder } from './SnakeBuilder.js';

describe('Self Collision', () => {
  it('snake colliding with its own body causes game over', () => {
    const game = new BoardBuilder()
      .withSize(5, 5)
      .withSnake(
        new SnakeBuilder()
          .withBodyAt([3, 0], [2, 0], [1, 0], [0, 0])
          .movingDown()
          .build(),
      )
      .withFoodAt(4, 4)
      .build();

    game.tick(); // head moves to (3,1)
    game.changeDirection('Left');
    game.tick(); // head moves to (2,1)
    game.changeDirection('Up');
    game.tick(); // head would move to (2,0) — body collision

    expect(game.state).toBe('GameOver');
  });
});
