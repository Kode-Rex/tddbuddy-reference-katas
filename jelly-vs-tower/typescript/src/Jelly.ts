import { ColorType } from './ColorType.js';
import { InvalidHealthException } from './InvalidHealthException.js';

export class Jelly {
  readonly id: string;
  readonly color: ColorType;
  private _health: number;

  constructor(id: string, color: ColorType, health: number) {
    if (health <= 0) throw new InvalidHealthException(health);
    this.id = id;
    this.color = color;
    this._health = health;
  }

  get health(): number {
    return this._health;
  }

  get isAlive(): boolean {
    return this._health > 0;
  }

  takeDamage(damage: number): void {
    this._health -= damage;
    if (this._health < 0) this._health = 0;
  }
}
