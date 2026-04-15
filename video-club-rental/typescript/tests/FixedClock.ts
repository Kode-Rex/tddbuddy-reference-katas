import type { Clock } from '../src/Clock.js';

const MS_PER_DAY = 86_400_000;

export class FixedClock implements Clock {
  private _today: Date;
  constructor(today: Date) { this._today = today; }
  today(): Date { return this._today; }
  advanceTo(d: Date): void { this._today = d; }
  advanceDays(days: number): void {
    this._today = new Date(this._today.getTime() + days * MS_PER_DAY);
  }
}
