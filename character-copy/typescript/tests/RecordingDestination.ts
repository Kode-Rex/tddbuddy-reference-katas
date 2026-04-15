import type { Destination } from '../src/Destination.js';

export class RecordingDestination implements Destination {
  private buffer = '';

  write(ch: string): void {
    this.buffer += ch;
  }

  get written(): string {
    return this.buffer;
  }
}
