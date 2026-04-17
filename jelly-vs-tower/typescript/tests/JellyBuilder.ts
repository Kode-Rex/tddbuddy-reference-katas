import { ColorType } from '../src/ColorType.js';
import { Jelly } from '../src/Jelly.js';

export class JellyBuilder {
  private _id = 'jelly-1';
  private _color: ColorType = ColorType.Blue;
  private _health = 10;

  withId(id: string): this { this._id = id; return this; }
  withColor(color: ColorType): this { this._color = color; return this; }
  withHealth(health: number): this { this._health = health; return this; }

  build(): Jelly {
    return new Jelly(this._id, this._color, this._health);
  }
}
