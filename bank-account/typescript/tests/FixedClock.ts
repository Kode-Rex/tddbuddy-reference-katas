import type { Clock } from '../src/Clock.js';

export class FixedClock implements Clock {
  constructor(private current: Date) {}
  today(): Date { return this.current; }
  advanceTo(date: Date): void { this.current = date; }
}
