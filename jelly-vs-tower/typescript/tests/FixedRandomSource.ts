import { RandomSource } from '../src/RandomSource.js';

export class FixedRandomSource implements RandomSource {
  private readonly value: number;

  constructor(value: number) {
    this.value = value;
  }

  next(_minInclusive: number, _maxInclusive: number): number {
    return this.value;
  }
}
