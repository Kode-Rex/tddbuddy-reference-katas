import { Library } from '../src/Library.js';
import { Member } from '../src/Member.js';
import { BookBuilder } from './BookBuilder.js';
import { FixedClock } from './FixedClock.js';
import { RecordingNotifier } from './RecordingNotifier.js';

export class LibraryBuilder {
  private _openedOn: Date = new Date(Date.UTC(2026, 0, 1));
  private _members: Member[] = [];
  private _books: BookBuilder[] = [];

  openedOn(d: Date): this { this._openedOn = d; return this; }
  withMember(m: Member): this { this._members.push(m); return this; }
  withBook(b: BookBuilder): this { this._books.push(b); return this; }

  build(): { library: Library; notifier: RecordingNotifier; clock: FixedClock } {
    const clock = new FixedClock(this._openedOn);
    const notifier = new RecordingNotifier();
    const library = new Library(clock, notifier);
    for (const m of this._members) library.seedMember(m);
    for (const b of this._books) library.seedBook(b.build(), b.copyCount);
    return { library, notifier, clock };
  }
}
