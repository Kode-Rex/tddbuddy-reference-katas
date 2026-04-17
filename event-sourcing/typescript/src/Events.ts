import { Money } from './Money.js';

export type AccountEventType = 'AccountOpened' | 'MoneyDeposited' | 'MoneyWithdrawn' | 'AccountClosed';

export interface AccountEvent {
  readonly type: AccountEventType;
  readonly accountId: string;
  readonly timestamp: Date;
}

export interface AccountOpened extends AccountEvent {
  readonly type: 'AccountOpened';
  readonly ownerName: string;
}

export interface MoneyDeposited extends AccountEvent {
  readonly type: 'MoneyDeposited';
  readonly amount: Money;
}

export interface MoneyWithdrawn extends AccountEvent {
  readonly type: 'MoneyWithdrawn';
  readonly amount: Money;
}

export interface AccountClosed extends AccountEvent {
  readonly type: 'AccountClosed';
}

export function accountOpened(accountId: string, ownerName: string, timestamp: Date): AccountOpened {
  return { type: 'AccountOpened', accountId, ownerName, timestamp };
}

export function moneyDeposited(accountId: string, amount: Money, timestamp: Date): MoneyDeposited {
  return { type: 'MoneyDeposited', accountId, amount, timestamp };
}

export function moneyWithdrawn(accountId: string, amount: Money, timestamp: Date): MoneyWithdrawn {
  return { type: 'MoneyWithdrawn', accountId, amount, timestamp };
}

export function accountClosed(accountId: string, timestamp: Date): AccountClosed {
  return { type: 'AccountClosed', accountId, timestamp };
}
