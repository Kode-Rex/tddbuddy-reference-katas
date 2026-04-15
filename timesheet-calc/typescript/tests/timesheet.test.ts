import { describe, it, expect } from 'vitest';
import { Day, STANDARD_WORK_WEEK_HOURS } from '../src/timesheet.js';
import { TimesheetBuilder } from './timesheetBuilder.js';

describe('Timesheet', () => {
  it('an empty timesheet totals zero across the board', () => {
    const totals = new TimesheetBuilder().build().totals();
    expect(totals.regularHours).toBe(0);
    expect(totals.overtimeHours).toBe(0);
    expect(totals.totalHours).toBe(0);
  });

  it('a single 8-hour weekday is all regular', () => {
    const totals = new TimesheetBuilder().withEntry(Day.Monday, 8).build().totals();
    expect(totals.regularHours).toBe(8);
    expect(totals.overtimeHours).toBe(0);
    expect(totals.totalHours).toBe(8);
  });

  it('a single weekday under 8 hours is all regular', () => {
    const totals = new TimesheetBuilder().withEntry(Day.Tuesday, 6).build().totals();
    expect(totals.regularHours).toBe(6);
    expect(totals.overtimeHours).toBe(0);
    expect(totals.totalHours).toBe(6);
  });

  it('weekday hours beyond 8 spill into overtime', () => {
    const totals = new TimesheetBuilder().withEntry(Day.Monday, 10).build().totals();
    expect(totals.regularHours).toBe(8);
    expect(totals.overtimeHours).toBe(2);
    expect(totals.totalHours).toBe(10);
  });

  it('fractional weekday overtime is tracked', () => {
    const totals = new TimesheetBuilder().withEntry(Day.Monday, 8.5).build().totals();
    expect(totals.regularHours).toBe(8);
    expect(totals.overtimeHours).toBeCloseTo(0.5, 9);
    expect(totals.totalHours).toBeCloseTo(8.5, 9);
  });

  it('Saturday hours are all overtime', () => {
    const totals = new TimesheetBuilder().withEntry(Day.Saturday, 4).build().totals();
    expect(totals.regularHours).toBe(0);
    expect(totals.overtimeHours).toBe(4);
    expect(totals.totalHours).toBe(4);
  });

  it('Sunday hours are all overtime', () => {
    const totals = new TimesheetBuilder().withEntry(Day.Sunday, 6).build().totals();
    expect(totals.regularHours).toBe(0);
    expect(totals.overtimeHours).toBe(6);
    expect(totals.totalHours).toBe(6);
  });

  it('a full Monday-to-Friday at 8 hours each totals the standard 40-hour week', () => {
    const totals = new TimesheetBuilder()
      .withEntry(Day.Monday, 8)
      .withEntry(Day.Tuesday, 8)
      .withEntry(Day.Wednesday, 8)
      .withEntry(Day.Thursday, 8)
      .withEntry(Day.Friday, 8)
      .build()
      .totals();
    expect(totals.regularHours).toBe(STANDARD_WORK_WEEK_HOURS);
    expect(totals.overtimeHours).toBe(0);
    expect(totals.totalHours).toBe(STANDARD_WORK_WEEK_HOURS);
  });

  it('a mixed week combines weekday overtime with weekend overtime', () => {
    const totals = new TimesheetBuilder()
      .withEntry(Day.Monday, 9)
      .withEntry(Day.Tuesday, 8)
      .withEntry(Day.Wednesday, 8)
      .withEntry(Day.Thursday, 8)
      .withEntry(Day.Friday, 10)
      .withEntry(Day.Saturday, 5)
      .build()
      .totals();
    expect(totals.regularHours).toBe(40);
    expect(totals.overtimeHours).toBe(8);
    expect(totals.totalHours).toBe(48);
  });

  it('later entries for the same day replace earlier entries', () => {
    const totals = new TimesheetBuilder()
      .withEntry(Day.Monday, 8)
      .withEntry(Day.Monday, 10)
      .build()
      .totals();
    expect(totals.regularHours).toBe(8);
    expect(totals.overtimeHours).toBe(2);
    expect(totals.totalHours).toBe(10);
  });

  it('a negative hours entry is rejected with the identical cross-language message', () => {
    expect(() => new TimesheetBuilder().withEntry(Day.Monday, -1).build())
      .toThrow('hours must not be negative');
  });

  it('a zero-hour entry is valid and contributes nothing', () => {
    const totals = new TimesheetBuilder().withEntry(Day.Monday, 0).build().totals();
    expect(totals.regularHours).toBe(0);
    expect(totals.overtimeHours).toBe(0);
    expect(totals.totalHours).toBe(0);
  });
});
