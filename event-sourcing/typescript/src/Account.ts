import type { AccountStatus } from './AccountStatus.js';
import { AccountSummary } from './AccountSummary.js';
import type { AccountEvent, AccountOpened, MoneyDeposited, MoneyWithdrawn } from './Events.js';
import { moneyDeposited, moneyWithdrawn, accountClosed } from './Events.js';
import {
  AccountNotOpenException,
  AccountClosedException,
  InsufficientFundsException,
  InvalidAmountException,
  NonZeroBalanceException,
} from './Exceptions.js';
import { Money } from './Money.js';
import { Transaction } from './Transaction.js';

export class Account {
  private readonly events: AccountEvent[] = [];
  private ownerName = '';
  private _balance: Money = Money.zero;
  private _status: AccountStatus = 'Open';
  private opened = false;

  readonly accountId: string;

  get balance(): Money { return this._balance; }
  get status(): AccountStatus { return this._status; }

  private constructor(accountId: string) {
    this.accountId = accountId;
  }

  static rebuild(events: AccountEvent[]): Account {
    if (events.length === 0) {
      throw new AccountNotOpenException('Account has no events.');
    }

    const first = events[0]!;
    if (first.type !== 'AccountOpened') {
      throw new AccountNotOpenException('Account was never opened.');
    }

    const account = new Account(first.accountId);
    for (const e of events) {
      account.apply(e);
    }
    return account;
  }

  deposit(amount: Money, timestamp: Date): AccountEvent {
    this.ensureOpen();
    if (!amount.isPositive) {
      throw new InvalidAmountException('Deposit amount must be positive.');
    }

    const e = moneyDeposited(this.accountId, amount, timestamp);
    this.apply(e);
    return e;
  }

  withdraw(amount: Money, timestamp: Date): AccountEvent {
    this.ensureOpen();
    if (!amount.isPositive) {
      throw new InvalidAmountException('Withdrawal amount must be positive.');
    }
    if (amount.greaterThan(this._balance)) {
      throw new InsufficientFundsException('Withdrawal amount exceeds current balance.');
    }

    const e = moneyWithdrawn(this.accountId, amount, timestamp);
    this.apply(e);
    return e;
  }

  close(timestamp: Date): AccountEvent {
    this.ensureOpen();
    if (!this._balance.equals(Money.zero)) {
      throw new NonZeroBalanceException('Cannot close account with non-zero balance.');
    }

    const e = accountClosed(this.accountId, timestamp);
    this.apply(e);
    return e;
  }

  transactionHistory(): Transaction[] {
    const transactions: Transaction[] = [];
    let running = Money.zero;
    for (const e of this.events) {
      if (e.type === 'MoneyDeposited') {
        const d = e as MoneyDeposited;
        running = running.plus(d.amount);
        transactions.push(new Transaction(d.timestamp, d.amount, running));
      } else if (e.type === 'MoneyWithdrawn') {
        const w = e as MoneyWithdrawn;
        running = running.minus(w.amount);
        transactions.push(new Transaction(w.timestamp, w.amount.negate(), running));
      }
    }
    return transactions;
  }

  summary(): AccountSummary {
    const txCount = this.events.filter(
      e => e.type === 'MoneyDeposited' || e.type === 'MoneyWithdrawn',
    ).length;
    return new AccountSummary(this.ownerName, this._balance, txCount, this._status);
  }

  balanceAt(pointInTime: Date): Money {
    let balance = Money.zero;
    for (const e of this.events) {
      if (e.timestamp > pointInTime) break;
      if (e.type === 'MoneyDeposited') {
        balance = balance.plus((e as MoneyDeposited).amount);
      } else if (e.type === 'MoneyWithdrawn') {
        balance = balance.minus((e as MoneyWithdrawn).amount);
      }
    }
    return balance;
  }

  transactionsInRange(from: Date, to: Date): Transaction[] {
    const transactions: Transaction[] = [];
    let running = Money.zero;
    for (const e of this.events) {
      if (e.type === 'MoneyDeposited') {
        const d = e as MoneyDeposited;
        running = running.plus(d.amount);
        if (d.timestamp >= from && d.timestamp <= to) {
          transactions.push(new Transaction(d.timestamp, d.amount, running));
        }
      } else if (e.type === 'MoneyWithdrawn') {
        const w = e as MoneyWithdrawn;
        running = running.minus(w.amount);
        if (w.timestamp >= from && w.timestamp <= to) {
          transactions.push(new Transaction(w.timestamp, w.amount.negate(), running));
        }
      }
    }
    return transactions;
  }

  private apply(e: AccountEvent): void {
    switch (e.type) {
      case 'AccountOpened': {
        const opened = e as AccountOpened;
        this.ownerName = opened.ownerName;
        this.opened = true;
        this._status = 'Open';
        break;
      }
      case 'MoneyDeposited':
        this._balance = this._balance.plus((e as MoneyDeposited).amount);
        break;
      case 'MoneyWithdrawn':
        this._balance = this._balance.minus((e as MoneyWithdrawn).amount);
        break;
      case 'AccountClosed':
        this._status = 'Closed';
        break;
    }
    this.events.push(e);
  }

  private ensureOpen(): void {
    if (!this.opened) {
      throw new AccountNotOpenException('Account was never opened.');
    }
    if (this._status === 'Closed') {
      throw new AccountClosedException('Account is closed.');
    }
  }
}
