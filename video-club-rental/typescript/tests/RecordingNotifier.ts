import type { Notifier } from '../src/Notifier.js';
import type { User } from '../src/User.js';

export interface Notification { user: User; message: string; }

export class RecordingNotifier implements Notifier {
  private _sent: Notification[] = [];
  get sent(): readonly Notification[] { return this._sent; }
  send(user: User, message: string): void { this._sent.push({ user, message }); }
  notificationsFor(user: User): Notification[] {
    return this._sent.filter((n) => n.user === user);
  }
}
