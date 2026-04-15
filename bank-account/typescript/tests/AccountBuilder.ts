import { Account } from '../src/Account.js';
import { Money } from '../src/Money.js';
import { FixedClock } from './FixedClock.js';

interface SeededDeposit { date: Date; amount: number; }

export class AccountBuilder {
  private _openedOn: Date = new Date(Date.UTC(2026, 0, 1));
  private _seededDeposits: SeededDeposit[] = [];

  openedOn(date: Date): this { this._openedOn = date; return this; }

  withDepositOn(date: Date, amount: number): this {
    this._seededDeposits.push({ date, amount });
    return this;
  }

  build(): { account: Account; clock: FixedClock } {
    const clock = new FixedClock(this._openedOn);
    const account = new Account(clock);
    for (const { date, amount } of this._seededDeposits) {
      clock.advanceTo(date);
      account.deposit(new Money(amount));
    }
    return { account, clock };
  }
}
