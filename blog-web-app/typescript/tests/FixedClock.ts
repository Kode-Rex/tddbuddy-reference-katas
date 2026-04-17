import type { Clock } from '../src/Clock.js';

export class FixedClock implements Clock {
  private _current: Date;

  constructor(current: Date) {
    this._current = current;
  }

  now(): Date {
    return this._current;
  }

  advanceTo(date: Date): void {
    this._current = date;
  }

  advanceByMinutes(minutes: number): void {
    this._current = new Date(this._current.getTime() + minutes * 60_000);
  }
}
