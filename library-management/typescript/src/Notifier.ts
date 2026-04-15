import type { Member } from './Member.js';

export interface Notifier {
  send(member: Member, message: string): void;
}
