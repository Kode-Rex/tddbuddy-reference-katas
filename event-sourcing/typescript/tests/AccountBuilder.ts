import { Account } from '../src/Account.js';
import type { AccountEvent } from '../src/Events.js';
import { accountOpened, moneyDeposited, moneyWithdrawn, accountClosed } from '../src/Events.js';
import { Money } from '../src/Money.js';

export class AccountBuilder {
  private static readonly T0 = new Date(Date.UTC(2026, 0, 1));

  private accountId = 'ACC-001';
  private ownerName = 'Alice';
  private openedAt = AccountBuilder.T0;
  private additionalEvents: AccountEvent[] = [];
  private timeStep = 0;

  withAccountId(id: string): this { this.accountId = id; return this; }
  withOwnerName(name: string): this { this.ownerName = name; return this; }
  openedAtTime(timestamp: Date): this { this.openedAt = timestamp; return this; }

  withDeposit(amount: number, timestamp?: Date): this {
    this.additionalEvents.push(
      moneyDeposited(this.accountId, new Money(amount), timestamp ?? this.nextTimestamp()),
    );
    return this;
  }

  withWithdrawal(amount: number, timestamp?: Date): this {
    this.additionalEvents.push(
      moneyWithdrawn(this.accountId, new Money(amount), timestamp ?? this.nextTimestamp()),
    );
    return this;
  }

  closed(timestamp?: Date): this {
    this.additionalEvents.push(
      accountClosed(this.accountId, timestamp ?? this.nextTimestamp()),
    );
    return this;
  }

  build(): Account {
    const events: AccountEvent[] = [
      accountOpened(this.accountId, this.ownerName, this.openedAt),
      ...this.additionalEvents,
    ];
    return Account.rebuild(events);
  }

  private nextTimestamp(): Date {
    this.timeStep++;
    return new Date(this.openedAt.getTime() + this.timeStep * 3600_000);
  }
}
