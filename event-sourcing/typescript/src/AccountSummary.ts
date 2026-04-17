import type { AccountStatus } from './AccountStatus.js';
import { Money } from './Money.js';

export class AccountSummary {
  constructor(
    public readonly ownerName: string,
    public readonly balance: Money,
    public readonly transactionCount: number,
    public readonly status: AccountStatus,
  ) {}
}
