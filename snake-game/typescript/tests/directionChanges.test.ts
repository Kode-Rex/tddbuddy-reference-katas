import { describe, it, expect } from 'vitest';
import { BoardBuilder } from './BoardBuilder.js';
import { SnakeBuilder } from './SnakeBuilder.js';

describe('Direction Changes', () => {
  it('changing direction to down then ticking moves the snake down', () => {
    const game = new BoardBuilder()
      .withSnake(new SnakeBuilder().at(1, 1).movingRight().build())
      .build();

    game.changeDirection('Down');
    game.tick();

    expect(game.snake.head).toEqual({ x: 1, y: 2 });
  });

  it('cannot reverse direction from right to left', () => {
    const game = new BoardBuilder()
      .withSnake(new SnakeBuilder().at(1, 0).movingRight().build())
      .build();

    game.changeDirection('Left');
    game.tick();

    expect(game.snake.head).toEqual({ x: 2, y: 0 });
  });

  it('cannot reverse direction from up to down', () => {
    const game = new BoardBuilder()
      .withSnake(new SnakeBuilder().at(0, 2).movingUp().build())
      .build();

    game.changeDirection('Down');
    game.tick();

    expect(game.snake.head).toEqual({ x: 0, y: 1 });
  });

  it('cannot reverse direction from left to right', () => {
    const game = new BoardBuilder()
      .withSnake(new SnakeBuilder().at(2, 0).movingLeft().build())
      .build();

    game.changeDirection('Right');
    game.tick();

    expect(game.snake.head).toEqual({ x: 1, y: 0 });
  });

  it('cannot reverse direction from down to up', () => {
    const game = new BoardBuilder()
      .withSnake(new SnakeBuilder().at(0, 1).movingDown().build())
      .build();

    game.changeDirection('Up');
    game.tick();

    expect(game.snake.head).toEqual({ x: 0, y: 2 });
  });
});
