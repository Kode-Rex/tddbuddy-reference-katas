import { ColorType } from '../src/ColorType.js';
import { Tower } from '../src/Tower.js';

export class TowerBuilder {
  private _id = 'tower-1';
  private _color: ColorType = ColorType.Blue;
  private _level = 1;

  withId(id: string): this { this._id = id; return this; }
  withColor(color: ColorType): this { this._color = color; return this; }
  withLevel(level: number): this { this._level = level; return this; }

  build(): Tower {
    return new Tower(this._id, this._color, this._level);
  }
}
