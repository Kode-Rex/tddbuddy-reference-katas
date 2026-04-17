export class Money {
  constructor(public readonly amount: number) {}
  static readonly zero = new Money(0);
  plus(other: Money): Money { return new Money(this.amount + other.amount); }
  lessThan(other: Money): boolean { return this.amount < other.amount; }
  equals(other: Money): boolean { return this.amount === other.amount; }
}
