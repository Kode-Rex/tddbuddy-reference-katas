import type { Clock } from '../src/Clock.js';

export class FixedClock implements Clock {
  private _now: Date;

  constructor(now: Date) {
    this._now = now;
  }

  now(): Date { return this._now; }

  advanceTo(date: Date): void { this._now = date; }
}
