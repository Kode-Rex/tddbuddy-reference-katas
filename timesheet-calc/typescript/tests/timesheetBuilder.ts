import { createTimesheet, Day, type Timesheet } from '../src/timesheet.js';

export class TimesheetBuilder {
  private readonly _entries = new Map<Day, number>();

  withEntry(day: Day, hours: number): this {
    this._entries.set(day, hours);
    return this;
  }

  build(): Timesheet {
    return createTimesheet(this._entries);
  }
}
