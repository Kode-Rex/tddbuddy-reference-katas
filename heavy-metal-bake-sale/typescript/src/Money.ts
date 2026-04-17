export class Money {
  static readonly zero = new Money(0);

  constructor(public readonly amount: number) {}

  plus(other: Money): Money {
    return new Money(Math.round((this.amount + other.amount) * 100) / 100);
  }

  minus(other: Money): Money {
    return new Money(Math.round((this.amount - other.amount) * 100) / 100);
  }

  lessThan(other: Money): boolean {
    return this.amount < other.amount;
  }

  equals(other: Money): boolean {
    return this.amount === other.amount;
  }

  toDisplay(): string {
    return `$${this.amount.toFixed(2)}`;
  }
}
