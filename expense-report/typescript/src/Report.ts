import type { ExpenseItem } from './ExpenseItem.js';
import { Money } from './Money.js';
import { ReportStatus } from './ReportStatus.js';
import { SpendingPolicy } from './SpendingPolicy.js';
import {
  EmptyReportError,
  FinalizedReportError,
  InvalidAmountError,
  InvalidStatusTransitionError,
  ReportExceedsMaximumError,
  ExpenseNotFoundError,
} from './Exceptions.js';

export class Report {
  private _expenses: ExpenseItem[] = [];
  private _status: ReportStatus = ReportStatus.Draft;
  private _rejectionReason: string | null = null;

  constructor(public readonly employeeName: string) {}

  get status(): ReportStatus { return this._status; }
  get expenses(): readonly ExpenseItem[] { return this._expenses; }
  get rejectionReason(): string | null { return this._rejectionReason; }

  get total(): Money {
    return this._expenses.reduce((sum, e) => sum.plus(e.amount), Money.zero);
  }

  get requiresApproval(): boolean {
    return this._expenses.some(e => e.isOverLimit) || this.total.greaterThan(SpendingPolicy.approvalThreshold);
  }

  get approvalReason(): string {
    if (this._expenses.some(e => e.isOverLimit)) return 'Yes (over-limit items)';
    if (this.total.greaterThan(SpendingPolicy.approvalThreshold)) return 'Yes (total exceeds $2,500)';
    return 'No';
  }

  addExpense(item: ExpenseItem): void {
    if (this._status === ReportStatus.Approved || this._status === ReportStatus.Rejected)
      throw new FinalizedReportError();
    if (!item.amount.isPositive)
      throw new InvalidAmountError();
    this._expenses.push(item);
  }

  removeExpense(item: ExpenseItem): void {
    const index = this._expenses.indexOf(item);
    if (index === -1) throw new ExpenseNotFoundError();
    this._expenses.splice(index, 1);
  }

  submit(): void {
    if (this._expenses.length === 0)
      throw new EmptyReportError();
    if (this.total.greaterThan(SpendingPolicy.reportMaximum))
      throw new ReportExceedsMaximumError();
    this._status = ReportStatus.Pending;
  }

  approve(): void {
    if (this._status !== ReportStatus.Pending)
      throw new InvalidStatusTransitionError('Only pending reports can be approved');
    this._status = ReportStatus.Approved;
  }

  reject(reason: string): void {
    if (this._status !== ReportStatus.Pending)
      throw new InvalidStatusTransitionError('Only pending reports can be rejected');
    this._status = ReportStatus.Rejected;
    this._rejectionReason = reason;
  }

  reopen(): void {
    if (this._status !== ReportStatus.Rejected)
      throw new InvalidStatusTransitionError('Only rejected reports can be reopened');
    this._status = ReportStatus.Draft;
    this._rejectionReason = null;
  }

  printSummary(): string {
    const lines: string[] = [];
    lines.push(`Expense Report: ${this.employeeName}`);
    lines.push(`Status: ${this._status}`);
    lines.push('');
    for (const e of this._expenses) {
      const flag = e.isOverLimit ? ' [OVER LIMIT]' : '';
      const amount = e.amount.format();
      lines.push(`  ${e.category}: ${e.description}  $${amount}${flag}`);
    }
    lines.push('');
    lines.push(`Total: $${this.total.format()}`);
    lines.push(`Requires Approval: ${this.approvalReason}`);
    return lines.join('\n');
  }
}
