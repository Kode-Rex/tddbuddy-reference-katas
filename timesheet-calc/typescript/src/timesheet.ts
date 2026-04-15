// Weekly timesheet with per-day hour entries classified as regular vs overtime.
// See ../SCENARIOS.md for the shared specification.

export enum Day {
  Monday = 'Monday',
  Tuesday = 'Tuesday',
  Wednesday = 'Wednesday',
  Thursday = 'Thursday',
  Friday = 'Friday',
  Saturday = 'Saturday',
  Sunday = 'Sunday',
}

export function isWeekend(day: Day): boolean {
  return day === Day.Saturday || day === Day.Sunday;
}

// Business numbers are named. F2 is Full-Bake — named constants win.
// Identical values across C#, TypeScript, and Python.
export const DAILY_OVERTIME_THRESHOLD = 8;
export const STANDARD_WORK_WEEK_HOURS = 40;

// The error message string is the spec — identical byte-for-byte across
// all three languages. The error type is language-idiomatic (Error here).
export const ERROR_HOURS_MUST_NOT_BE_NEGATIVE = 'hours must not be negative';

export interface TimesheetTotals {
  readonly regularHours: number;
  readonly overtimeHours: number;
  readonly totalHours: number;
}

export interface Timesheet {
  totals(): TimesheetTotals;
}

export function createTimesheet(entries: ReadonlyMap<Day, number>): Timesheet {
  for (const hours of entries.values()) {
    if (hours < 0) {
      throw new Error(ERROR_HOURS_MUST_NOT_BE_NEGATIVE);
    }
  }

  return {
    totals(): TimesheetTotals {
      let regularHours = 0;
      let overtimeHours = 0;

      for (const [day, hours] of entries) {
        if (isWeekend(day)) {
          overtimeHours += hours;
        } else if (hours > DAILY_OVERTIME_THRESHOLD) {
          regularHours += DAILY_OVERTIME_THRESHOLD;
          overtimeHours += hours - DAILY_OVERTIME_THRESHOLD;
        } else {
          regularHours += hours;
        }
      }

      return {
        regularHours,
        overtimeHours,
        totalHours: regularHours + overtimeHours,
      };
    },
  };
}
