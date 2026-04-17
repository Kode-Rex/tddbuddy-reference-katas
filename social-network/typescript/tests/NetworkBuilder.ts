import { Network } from '../src/Network.js';
import { FixedClock } from './FixedClock.js';

interface SeededPost {
  user: string;
  content: string;
  minutesAfterStart: number;
}

interface SeededFollow {
  follower: string;
  followee: string;
}

export class NetworkBuilder {
  private _startTime = new Date(Date.UTC(2026, 0, 15, 9, 0, 0));
  private _seededPosts: SeededPost[] = [];
  private _seededFollows: SeededFollow[] = [];

  startingAt(date: Date): this {
    this._startTime = date;
    return this;
  }

  withPost(user: string, content: string, minutesAfterStart = 0): this {
    this._seededPosts.push({ user, content, minutesAfterStart });
    return this;
  }

  withFollow(follower: string, followee: string): this {
    this._seededFollows.push({ follower, followee });
    return this;
  }

  build(): { network: Network; clock: FixedClock } {
    const clock = new FixedClock(this._startTime);
    const network = new Network(clock);

    for (const { user, content, minutesAfterStart } of this._seededPosts) {
      clock.advanceTo(new Date(this._startTime.getTime() + minutesAfterStart * 60_000));
      network.post(user, content);
    }

    for (const { follower, followee } of this._seededFollows) {
      network.follow(follower, followee);
    }

    return { network, clock };
  }
}
