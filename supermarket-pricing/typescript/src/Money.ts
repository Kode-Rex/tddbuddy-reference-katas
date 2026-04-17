/**
 * Monetary amount in cents. Integer arithmetic avoids floating-point rounding.
 */
export class Money {
  constructor(public readonly cents: number) {}

  static readonly zero = new Money(0);

  plus(other: Money): Money { return new Money(this.cents + other.cents); }
  minus(other: Money): Money { return new Money(this.cents - other.cents); }
  times(factor: number): Money { return new Money(this.cents * factor); }
  equals(other: Money): boolean { return this.cents === other.cents; }
}
