export class Digit {
  private constructor(public readonly value: number | null) {}

  static of(value: number): Digit {
    if (value < 0 || value > 9 || !Number.isInteger(value)) {
      throw new RangeError('Digit value must be an integer 0-9.');
    }
    return new Digit(value);
  }

  static readonly unknown: Digit = new Digit(null);

  get isKnown(): boolean { return this.value !== null; }

  equals(other: Digit): boolean { return this.value === other.value; }

  toString(): string { return this.isKnown ? String(this.value) : '?'; }
}
