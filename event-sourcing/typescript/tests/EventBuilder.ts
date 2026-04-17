import { Money } from '../src/Money.js';
import {
  accountOpened,
  moneyDeposited,
  moneyWithdrawn,
  accountClosed,
  type AccountOpened,
  type MoneyDeposited,
  type MoneyWithdrawn,
  type AccountClosed,
} from '../src/Events.js';

const DefaultAccountId = 'ACC-001';
const DefaultOwnerName = 'Alice';
const DefaultTimestamp = new Date(Date.UTC(2026, 0, 1));

export function anAccountOpened(opts?: {
  accountId?: string;
  ownerName?: string;
  timestamp?: Date;
}): AccountOpened {
  return accountOpened(
    opts?.accountId ?? DefaultAccountId,
    opts?.ownerName ?? DefaultOwnerName,
    opts?.timestamp ?? DefaultTimestamp,
  );
}

export function aMoneyDeposited(
  amount: number,
  opts?: { accountId?: string; timestamp?: Date },
): MoneyDeposited {
  return moneyDeposited(
    opts?.accountId ?? DefaultAccountId,
    new Money(amount),
    opts?.timestamp ?? DefaultTimestamp,
  );
}

export function aMoneyWithdrawn(
  amount: number,
  opts?: { accountId?: string; timestamp?: Date },
): MoneyWithdrawn {
  return moneyWithdrawn(
    opts?.accountId ?? DefaultAccountId,
    new Money(amount),
    opts?.timestamp ?? DefaultTimestamp,
  );
}

export function anAccountClosed(opts?: {
  accountId?: string;
  timestamp?: Date;
}): AccountClosed {
  return accountClosed(
    opts?.accountId ?? DefaultAccountId,
    opts?.timestamp ?? DefaultTimestamp,
  );
}
