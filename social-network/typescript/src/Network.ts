import type { Clock } from './Clock.js';
import { Post } from './Post.js';
import { User } from './User.js';

export class Network {
  private readonly _users = new Map<string, User>();
  private readonly _posts: Post[] = [];

  constructor(private readonly clock: Clock) {}

  get users(): ReadonlyMap<string, User> {
    return this._users;
  }

  post(userName: string, content: string): void {
    this.ensureRegistered(userName);
    this._posts.push(new Post(userName, content, this.clock.now()));
  }

  follow(followerName: string, followeeName: string): void {
    this.ensureRegistered(followerName);
    this.ensureRegistered(followeeName);
    this._users.get(followerName)!.follow(followeeName);
  }

  timeline(userName: string): readonly Post[] {
    return this._posts
      .filter((p) => p.author === userName)
      .sort((a, b) => b.timestamp.getTime() - a.timestamp.getTime());
  }

  wall(userName: string): readonly Post[] {
    const user = this._users.get(userName);
    if (!user) return [];

    const visible = new Set<string>([userName]);
    for (const followed of user.following) {
      visible.add(followed);
    }

    return this._posts
      .filter((p) => visible.has(p.author))
      .sort((a, b) => b.timestamp.getTime() - a.timestamp.getTime());
  }

  private ensureRegistered(userName: string): void {
    if (!this._users.has(userName)) {
      this._users.set(userName, new User(userName));
    }
  }
}
