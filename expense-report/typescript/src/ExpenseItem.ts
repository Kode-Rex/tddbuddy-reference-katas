import { Category } from './Category.js';
import { Money } from './Money.js';
import { SpendingPolicy } from './SpendingPolicy.js';

export class ExpenseItem {
  constructor(
    public readonly description: string,
    public readonly amount: Money,
    public readonly category: Category,
  ) {}

  get isOverLimit(): boolean {
    return this.amount.greaterThan(SpendingPolicy.limitFor(this.category));
  }
}
