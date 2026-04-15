import { Title } from '../src/Title.js';
import { User } from '../src/User.js';
import { VideoClub } from '../src/VideoClub.js';
import { FixedClock } from './FixedClock.js';
import { RecordingNotifier } from './RecordingNotifier.js';

export class VideoClubBuilder {
  private _openedOn: Date = new Date(Date.UTC(2026, 0, 1));
  private _users: User[] = [];
  private _titles: Title[] = [];

  openedOn(date: Date): this { this._openedOn = date; return this; }
  withUser(user: User): this { this._users.push(user); return this; }
  withTitle(title: Title): this { this._titles.push(title); return this; }

  build(): { club: VideoClub; notifier: RecordingNotifier; clock: FixedClock } {
    const clock = new FixedClock(this._openedOn);
    const notifier = new RecordingNotifier();
    const club = new VideoClub(clock, notifier);
    for (const u of this._users) club.seedUser(u);
    for (const t of this._titles) club.addTitle(t);
    return { club, notifier, clock };
  }
}
