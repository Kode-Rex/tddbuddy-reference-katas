import { CopyStatus } from './CopyStatus.js';
import { Isbn } from './Isbn.js';

export class Copy {
  private _status: CopyStatus = CopyStatus.Available;

  constructor(public readonly id: number, public readonly isbn: Isbn) {}

  get status(): CopyStatus { return this._status; }

  markCheckedOut(): void { this._status = CopyStatus.CheckedOut; }
  markAvailable(): void { this._status = CopyStatus.Available; }
  markReserved(): void { this._status = CopyStatus.Reserved; }
}
