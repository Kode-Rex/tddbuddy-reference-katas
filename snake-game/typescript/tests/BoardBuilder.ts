import type { Position } from '../src/Position.js';
import type { FoodSpawner } from '../src/Game.js';
import { Game } from '../src/Game.js';
import { Snake } from '../src/Snake.js';

export class BoardBuilder {
  private _width = 5;
  private _height = 5;
  private _snake?: Snake;
  private _food?: Position;
  private _foodSpawner?: FoodSpawner;

  withSize(width: number, height: number): this {
    this._width = width;
    this._height = height;
    return this;
  }

  withSnake(snake: Snake): this {
    this._snake = snake;
    return this;
  }

  withFoodAt(x: number, y: number): this {
    this._food = { x, y };
    return this;
  }

  withFoodSpawner(spawner: FoodSpawner): this {
    this._foodSpawner = spawner;
    return this;
  }

  build(): Game {
    const snake = this._snake ?? new Snake([{ x: 0, y: 0 }], 'Right');
    const food = this._food ?? { x: this._width - 1, y: this._height - 1 };
    const spawner = this._foodSpawner ?? ((cells) => cells[0]!);

    return new Game(this._width, this._height, spawner, snake, food, 0, 'Playing');
  }
}
