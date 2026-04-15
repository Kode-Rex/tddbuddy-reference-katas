export class Money {
  constructor(public readonly amount: number) {}
  static readonly zero = new Money(0);
  plus(other: Money): Money { return new Money(round2(this.amount + other.amount)); }
  equals(other: Money): boolean { return round2(this.amount) === round2(other.amount); }
  toString(): string { return `£${this.amount.toFixed(2)}`; }
}

function round2(n: number): number { return Math.round(n * 100) / 100; }
