import { Copy } from './Copy.js';
import { Member } from './Member.js';
import { Money } from './Money.js';

export const LOAN_PERIOD_DAYS = 14;
export const FINE_PER_DAY = new Money(0.10);

const MS_PER_DAY = 86_400_000;

export class Loan {
  public readonly dueOn: Date;
  private _returnedOn: Date | null = null;

  constructor(
    public readonly member: Member,
    public readonly copy: Copy,
    public readonly borrowedOn: Date,
  ) {
    this.dueOn = new Date(borrowedOn.getTime() + LOAN_PERIOD_DAYS * MS_PER_DAY);
  }

  get returnedOn(): Date | null { return this._returnedOn; }
  get isClosed(): boolean { return this._returnedOn !== null; }

  fineFor(returnDate: Date): Money {
    if (returnDate.getTime() <= this.dueOn.getTime()) return Money.zero;
    const daysLate = Math.round((returnDate.getTime() - this.dueOn.getTime()) / MS_PER_DAY);
    return FINE_PER_DAY.times(daysLate);
  }

  close(returnedOn: Date): void { this._returnedOn = returnedOn; }
}
