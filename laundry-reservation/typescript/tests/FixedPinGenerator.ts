import type { PinGenerator } from '../src/PinGenerator.js';

export class FixedPinGenerator implements PinGenerator {
  private readonly pins: number[];
  private index = 0;

  constructor(...pins: number[]) {
    this.pins = pins;
  }

  generate(): number {
    return this.pins[this.index++]!;
  }
}
