import type { Clock } from '../src/Clock.js';

export class FixedClock implements Clock {
  private current: Date;

  constructor(start: Date) {
    this.current = start;
  }

  now(): Date {
    return this.current;
  }

  advanceTo(when: Date): void {
    this.current = when;
  }

  advanceMs(ms: number): void {
    this.current = new Date(this.current.getTime() + ms);
  }
}
