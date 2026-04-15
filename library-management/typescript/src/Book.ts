import { Copy } from './Copy.js';
import { CopyStatus } from './CopyStatus.js';
import { Isbn } from './Isbn.js';

export class Book {
  private readonly _copies: Copy[] = [];
  private _nextCopyId = 1;

  constructor(
    public readonly title: string,
    public readonly author: string,
    public readonly isbn: Isbn,
  ) {}

  get copies(): readonly Copy[] { return this._copies; }
  get copyCount(): number { return this._copies.length; }

  addCopy(): Copy {
    const copy = new Copy(this._nextCopyId++, this.isbn);
    this._copies.push(copy);
    return copy;
  }

  removeOneCopy(): void {
    const available = this._copies.findIndex((c) => c.status === CopyStatus.Available);
    const idx = available >= 0 ? available : this._copies.length > 0 ? 0 : -1;
    if (idx >= 0) this._copies.splice(idx, 1);
  }

  findAvailableCopy(): Copy | undefined {
    return this._copies.find((c) => c.status === CopyStatus.Available);
  }

  findReservedCopy(): Copy | undefined {
    return this._copies.find((c) => c.status === CopyStatus.Reserved);
  }
}
