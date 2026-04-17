export class Money {
  constructor(public readonly amount: number) {}
  static readonly zero = new Money(0);
  get isPositive(): boolean { return this.amount > 0; }
  plus(other: Money): Money { return new Money(this.amount + other.amount); }
  minus(other: Money): Money { return new Money(this.amount - other.amount); }
  negate(): Money { return new Money(-this.amount); }
  greaterThan(other: Money): boolean { return this.amount > other.amount; }
  equals(other: Money): boolean { return this.amount === other.amount; }
}
