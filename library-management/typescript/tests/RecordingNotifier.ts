import type { Member } from '../src/Member.js';
import type { Notifier } from '../src/Notifier.js';

export interface Notification { member: Member; message: string; }

export class RecordingNotifier implements Notifier {
  private _sent: Notification[] = [];
  get sent(): readonly Notification[] { return this._sent; }
  send(member: Member, message: string): void { this._sent.push({ member, message }); }
  notificationsFor(member: Member): Notification[] {
    return this._sent.filter((n) => n.member === member);
  }
  availabilityNotificationsFor(member: Member): Notification[] {
    return this.notificationsFor(member).filter((n) => n.message.includes('available'));
  }
  expirationNotificationsFor(member: Member): Notification[] {
    return this.notificationsFor(member).filter((n) => n.message.includes('expired'));
  }
}
