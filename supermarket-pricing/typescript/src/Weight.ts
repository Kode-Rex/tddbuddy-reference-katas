/**
 * Non-negative weight in kilograms.
 */
export class Weight {
  readonly kg: number;

  constructor(kg: number) {
    if (kg < 0) {
      throw new RangeError('Weight must not be negative');
    }
    this.kg = kg;
  }

  static readonly zero = new Weight(0);
}
