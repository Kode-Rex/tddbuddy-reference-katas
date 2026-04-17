import { describe, expect, it } from 'vitest';
import { Category } from '../src/Category.js';
import { Money } from '../src/Money.js';
import { ReportStatus } from '../src/ReportStatus.js';
import { SpendingPolicy } from '../src/SpendingPolicy.js';
import {
  EmptyReportError,
  FinalizedReportError,
  InvalidAmountError,
  InvalidStatusTransitionError,
  ReportExceedsMaximumError,
} from '../src/Exceptions.js';
import { ExpenseItemBuilder } from './ExpenseItemBuilder.js';
import { ReportBuilder } from './ReportBuilder.js';

describe('Report', () => {
  // --- Expense Items ---

  it('an expense item within its category limit is not flagged as over-limit', () => {
    const item = new ExpenseItemBuilder().asMeal(45).build();
    expect(item.isOverLimit).toBe(false);
  });

  it('an expense item exceeding its category limit is flagged as over-limit', () => {
    const item = new ExpenseItemBuilder().asMeal(60).build();
    expect(item.isOverLimit).toBe(true);
  });

  it.each([
    [Category.Meals, 50],
    [Category.Travel, 500],
    [Category.Accommodation, 200],
    [Category.Equipment, 1000],
    [Category.Other, 100],
  ])('category %s has per-item limit of $%d', (category, expected) => {
    expect(SpendingPolicy.limitFor(category).amount).toBe(expected);
  });

  // --- Creating and Adding Expenses ---

  it('a new report starts in Draft status', () => {
    const report = new ReportBuilder().build();
    expect(report.status).toBe(ReportStatus.Draft);
  });

  it('adding an expense to a draft report increases the total', () => {
    const report = new ReportBuilder().build();
    report.addExpense(new ExpenseItemBuilder().asMeal(30).build());
    expect(report.total.amount).toBe(30);
  });

  it('adding an expense with zero amount is rejected', () => {
    const report = new ReportBuilder().build();
    expect(() => report.addExpense(new ExpenseItemBuilder().withAmount(0).build()))
      .toThrow(InvalidAmountError);
  });

  it('adding an expense with negative amount is rejected', () => {
    const report = new ReportBuilder().build();
    expect(() => report.addExpense(new ExpenseItemBuilder().withAmount(-10).build()))
      .toThrow(InvalidAmountError);
  });

  // --- Submitting ---

  it('submitting a draft report moves it to Pending', () => {
    const report = new ReportBuilder()
      .withExpenseFrom(b => b.asMeal(30))
      .build();
    report.submit();
    expect(report.status).toBe(ReportStatus.Pending);
  });

  it('submitting an empty report is rejected', () => {
    const report = new ReportBuilder().build();
    expect(() => report.submit()).toThrow(EmptyReportError);
  });

  it('submitting a report over $5,000 total is rejected', () => {
    const builder = new ReportBuilder();
    for (let i = 0; i < 6; i++) builder.withExpenseFrom(b => b.asEquipment(1000));
    const report = builder.build();
    expect(() => report.submit()).toThrow(ReportExceedsMaximumError);
  });

  // --- Approval Rules ---

  it('a report under $2,500 with no over-limit items does not require approval', () => {
    const report = new ReportBuilder()
      .withExpenseFrom(b => b.asMeal(30))
      .withExpenseFrom(b => b.asTravel(200))
      .build();
    expect(report.requiresApproval).toBe(false);
    expect(report.approvalReason).toBe('No');
  });

  it('a report with an over-limit item requires approval', () => {
    const report = new ReportBuilder()
      .withExpenseFrom(b => b.asMeal(60))
      .build();
    expect(report.requiresApproval).toBe(true);
    expect(report.approvalReason).toBe('Yes (over-limit items)');
  });

  it('a report over $2,500 total requires approval regardless of individual items', () => {
    const builder = new ReportBuilder();
    for (let i = 0; i < 6; i++) builder.withExpenseFrom(b => b.asTravel(500));
    const report = builder.build();
    expect(report.requiresApproval).toBe(true);
    expect(report.approvalReason).toBe('Yes (total exceeds $2,500)');
  });

  // --- Approve and Reject ---

  it('approving a pending report moves it to Approved', () => {
    const report = new ReportBuilder()
      .withExpenseFrom(b => b.asMeal(30))
      .submitted()
      .build();
    report.approve();
    expect(report.status).toBe(ReportStatus.Approved);
  });

  it('approving a non-pending report is rejected', () => {
    const report = new ReportBuilder().build();
    expect(() => report.approve()).toThrow(InvalidStatusTransitionError);
  });

  it('rejecting a pending report with a reason moves it to Rejected', () => {
    const report = new ReportBuilder()
      .withExpenseFrom(b => b.asMeal(30))
      .submitted()
      .build();
    report.reject('Over budget');
    expect(report.status).toBe(ReportStatus.Rejected);
    expect(report.rejectionReason).toBe('Over budget');
  });

  it('rejecting a non-pending report is rejected', () => {
    const report = new ReportBuilder().build();
    expect(() => report.reject('Reason')).toThrow(InvalidStatusTransitionError);
  });

  // --- Finalized Report Constraints ---

  it('adding an expense to an approved report is rejected', () => {
    const report = new ReportBuilder()
      .withExpenseFrom(b => b.asMeal(30))
      .approved()
      .build();
    expect(() => report.addExpense(new ExpenseItemBuilder().asMeal(20).build()))
      .toThrow(FinalizedReportError);
  });

  it('adding an expense to a rejected report is rejected', () => {
    const report = new ReportBuilder()
      .withExpenseFrom(b => b.asMeal(30))
      .rejected()
      .build();
    expect(() => report.addExpense(new ExpenseItemBuilder().asMeal(20).build()))
      .toThrow(FinalizedReportError);
  });

  // --- Reopen ---

  it('reopening a rejected report moves it back to Draft', () => {
    const report = new ReportBuilder()
      .withExpenseFrom(b => b.asMeal(30))
      .rejected('Policy violation')
      .build();
    report.reopen();
    expect(report.status).toBe(ReportStatus.Draft);
  });

  it('reopening a non-rejected report is rejected', () => {
    const report = new ReportBuilder()
      .withExpenseFrom(b => b.asMeal(30))
      .submitted()
      .build();
    expect(() => report.reopen()).toThrow(InvalidStatusTransitionError);
  });

  // --- Summary Output ---

  it('summary lists each expense with category, description, amount, and over-limit flag', () => {
    const report = new ReportBuilder()
      .withEmployeeName('Alice Johnson')
      .withExpenseFrom(b => b.asMeal(45).withDescription('Team lunch'))
      .withExpenseFrom(b => b.asTravel(350).withDescription('Flight to NYC'))
      .withExpenseFrom(b => b.asMeal(62).withDescription('Client dinner'))
      .withExpenseFrom(b => b.asEquipment(1200).withDescription('Laptop'))
      .build();

    report.submit();
    const summary = report.printSummary();

    expect(summary).toContain('Expense Report: Alice Johnson');
    expect(summary).toContain('Status: Pending');
    expect(summary).toContain('Meals: Team lunch');
    expect(summary).toContain('$45.00');
    expect(summary).toContain('Travel: Flight to NYC');
    expect(summary).toContain('$350.00');
    expect(summary).toContain('Client dinner  $62.00 [OVER LIMIT]');
    expect(summary).toContain('Laptop  $1,200.00 [OVER LIMIT]');
    expect(summary).toContain('Total: $1,657.00');
    expect(summary).toContain('Requires Approval: Yes (over-limit items)');
  });
});
