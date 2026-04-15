import type { Source } from './Source.js';
import type { Destination } from './Destination.js';

export function copy(source: Source, destination: Destination): void {
  while (true) {
    const ch = source.read();
    if (ch === '\n') return;
    destination.write(ch);
  }
}
