import { describe, it, expect } from 'vitest';
import { BoardBuilder } from './BoardBuilder.js';
import { SnakeBuilder } from './SnakeBuilder.js';

describe('Eating Food', () => {
  it('snake eats food and grows by one segment', () => {
    const game = new BoardBuilder()
      .withSnake(new SnakeBuilder().at(0, 0).movingRight().build())
      .withFoodAt(1, 0)
      .build();

    game.tick();

    expect(game.snake.length).toBe(2);
    expect(game.snake.head).toEqual({ x: 1, y: 0 });
    expect(game.snake.body).toEqual([
      { x: 1, y: 0 },
      { x: 0, y: 0 },
    ]);
  });

  it('eating food increments the score', () => {
    const game = new BoardBuilder()
      .withSnake(new SnakeBuilder().at(0, 0).movingRight().build())
      .withFoodAt(1, 0)
      .build();

    game.tick();

    expect(game.score).toBe(1);
  });

  it('new food spawns after eating at the position chosen by the spawner', () => {
    const nextFoodPosition = { x: 3, y: 3 };
    const game = new BoardBuilder()
      .withSnake(new SnakeBuilder().at(0, 0).movingRight().build())
      .withFoodAt(1, 0)
      .withFoodSpawner(() => nextFoodPosition)
      .build();

    game.tick();

    expect(game.food).toEqual(nextFoodPosition);
  });
});
