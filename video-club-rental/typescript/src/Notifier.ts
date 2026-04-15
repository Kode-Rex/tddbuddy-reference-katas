import type { User } from './User.js';

export interface Notifier {
  send(user: User, message: string): void;
}
