import type { Position } from '../src/Position.js';
import type { Direction } from '../src/Direction.js';
import { Snake } from '../src/Snake.js';

export class SnakeBuilder {
  private _body: Position[] = [{ x: 0, y: 0 }];
  private _direction: Direction = 'Right';

  at(x: number, y: number): this {
    this._body = [{ x, y }];
    return this;
  }

  withBodyAt(...positions: [number, number][]): this {
    this._body = positions.map(([x, y]) => ({ x, y }));
    return this;
  }

  movingUp(): this { this._direction = 'Up'; return this; }
  movingDown(): this { this._direction = 'Down'; return this; }
  movingLeft(): this { this._direction = 'Left'; return this; }
  movingRight(): this { this._direction = 'Right'; return this; }

  build(): Snake {
    return new Snake(this._body, this._direction);
  }
}
