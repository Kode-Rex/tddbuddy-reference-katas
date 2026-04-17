import { ColorType } from './ColorType.js';
import { InvalidLevelException } from './InvalidLevelException.js';

export class Tower {
  readonly id: string;
  readonly color: ColorType;
  readonly level: number;

  constructor(id: string, color: ColorType, level: number) {
    if (level < 1 || level > 4) throw new InvalidLevelException(level);
    this.id = id;
    this.color = color;
    this.level = level;
  }
}
