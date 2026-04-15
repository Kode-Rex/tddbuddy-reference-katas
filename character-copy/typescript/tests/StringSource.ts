import type { Source } from '../src/Source.js';

export class StringSource implements Source {
  private index = 0;
  public readCount = 0;

  constructor(private readonly payload: string) {}

  read(): string {
    this.readCount++;
    if (this.index >= this.payload.length) return '\n';
    return this.payload.charAt(this.index++);
  }
}
