export class Quantity {
  readonly value: number;

  constructor(value: number) {
    if (!Number.isInteger(value) || value <= 0) {
      throw new RangeError('Quantity must be a positive whole number');
    }
    this.value = value;
  }

  plus(other: Quantity): Quantity { return new Quantity(this.value + other.value); }
}
