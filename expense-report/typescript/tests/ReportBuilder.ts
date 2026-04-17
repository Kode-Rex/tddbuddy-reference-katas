import type { ExpenseItem } from '../src/ExpenseItem.js';
import { Report } from '../src/Report.js';
import { ExpenseItemBuilder } from './ExpenseItemBuilder.js';

export class ReportBuilder {
  private _employeeName = 'Alice Johnson';
  private _expenses: ExpenseItem[] = [];
  private _submit = false;
  private _approve = false;
  private _rejectReason: string | null = null;

  withEmployeeName(name: string): this { this._employeeName = name; return this; }

  withExpense(item: ExpenseItem): this { this._expenses.push(item); return this; }

  withExpenseFrom(configure: (b: ExpenseItemBuilder) => void): this {
    const builder = new ExpenseItemBuilder();
    configure(builder);
    this._expenses.push(builder.build());
    return this;
  }

  submitted(): this { this._submit = true; return this; }

  approved(): this { this._submit = true; this._approve = true; return this; }

  rejected(reason = 'Policy violation'): this {
    this._submit = true;
    this._rejectReason = reason;
    return this;
  }

  build(): Report {
    const report = new Report(this._employeeName);
    for (const expense of this._expenses) {
      report.addExpense(expense);
    }
    if (this._submit) report.submit();
    if (this._approve) report.approve();
    if (this._rejectReason !== null) report.reject(this._rejectReason);
    return report;
  }
}
