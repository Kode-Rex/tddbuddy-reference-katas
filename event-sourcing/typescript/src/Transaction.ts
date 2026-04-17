import { Money } from './Money.js';

export class Transaction {
  constructor(
    public readonly timestamp: Date,
    public readonly amount: Money,
    public readonly balanceAfter: Money,
  ) {}
}
