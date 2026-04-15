import type { Clock } from './Clock.js';
import { Money } from './Money.js';
import { Transaction } from './Transaction.js';

function formatDate(d: Date): string {
  const y = d.getUTCFullYear();
  const m = String(d.getUTCMonth() + 1).padStart(2, '0');
  const day = String(d.getUTCDate()).padStart(2, '0');
  return `${y}-${m}-${day}`;
}

function formatAmount(amount: number): string {
  return amount.toFixed(2).padStart(7, ' ');
}

export class Account {
  private _balance: Money = Money.zero;
  private _transactions: Transaction[] = [];

  constructor(private readonly clock: Clock) {}

  get balance(): Money { return this._balance; }
  get transactions(): readonly Transaction[] { return this._transactions; }

  deposit(amount: Money): boolean {
    if (!amount.isPositive) return false;
    this._balance = this._balance.plus(amount);
    this._transactions.push(new Transaction(this.clock.today(), amount, this._balance));
    return true;
  }

  withdraw(amount: Money): boolean {
    if (!amount.isPositive) return false;
    if (amount.greaterThan(this._balance)) return false;
    this._balance = this._balance.minus(amount);
    this._transactions.push(
      new Transaction(this.clock.today(), new Money(-amount.amount), this._balance),
    );
    return true;
  }

  printStatement(): string {
    const header = 'Date       | Amount  | Balance';
    const rows = this._transactions.map((t) =>
      `${formatDate(t.date)} | ${formatAmount(t.amount.amount)} | ${formatAmount(t.balanceAfter.amount)}`,
    );
    return [header, ...rows].join('\n');
  }
}
