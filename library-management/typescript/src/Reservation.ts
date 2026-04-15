import { Isbn } from './Isbn.js';
import { Member } from './Member.js';

export const RESERVATION_EXPIRY_DAYS = 3;

const MS_PER_DAY = 86_400_000;

export class Reservation {
  private _notifiedOn: Date | null = null;

  constructor(
    public readonly member: Member,
    public readonly isbn: Isbn,
    public readonly reservedOn: Date,
  ) {}

  get notifiedOn(): Date | null { return this._notifiedOn; }
  get isNotified(): boolean { return this._notifiedOn !== null; }

  hasExpiredAt(today: Date): boolean {
    if (this._notifiedOn === null) return false;
    const daysSince = Math.round((today.getTime() - this._notifiedOn.getTime()) / MS_PER_DAY);
    return daysSince > RESERVATION_EXPIRY_DAYS;
  }

  markNotified(today: Date): void { this._notifiedOn = today; }
}
