import { describe, it, expect } from 'vitest';
import { BoardBuilder } from './BoardBuilder.js';
import { SnakeBuilder } from './SnakeBuilder.js';

describe('Basic Movement', () => {
  it('snake moving right advances x by one', () => {
    const game = new BoardBuilder()
      .withSnake(new SnakeBuilder().at(0, 0).movingRight().build())
      .build();

    game.tick();

    expect(game.snake.head).toEqual({ x: 1, y: 0 });
  });

  it('snake moving down advances y by one', () => {
    const game = new BoardBuilder()
      .withSnake(new SnakeBuilder().at(0, 0).movingDown().build())
      .build();

    game.tick();

    expect(game.snake.head).toEqual({ x: 0, y: 1 });
  });

  it('snake moving left decreases x by one', () => {
    const game = new BoardBuilder()
      .withSnake(new SnakeBuilder().at(2, 0).movingLeft().build())
      .build();

    game.tick();

    expect(game.snake.head).toEqual({ x: 1, y: 0 });
  });

  it('snake moving up decreases y by one', () => {
    const game = new BoardBuilder()
      .withSnake(new SnakeBuilder().at(0, 2).movingUp().build())
      .build();

    game.tick();

    expect(game.snake.head).toEqual({ x: 0, y: 1 });
  });
});
