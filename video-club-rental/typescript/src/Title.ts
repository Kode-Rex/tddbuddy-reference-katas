export class Title {
  private _totalCopies: number;
  private _availableCopies: number;

  constructor(public readonly name: string, totalCopies: number) {
    if (totalCopies < 0) throw new Error('totalCopies must be non-negative');
    this._totalCopies = totalCopies;
    this._availableCopies = totalCopies;
  }

  get totalCopies(): number { return this._totalCopies; }
  get availableCopies(): number { return this._availableCopies; }

  addCopy(): void {
    this._totalCopies++;
    this._availableCopies++;
  }
  checkOut(): void {
    if (this._availableCopies <= 0) throw new Error(`No copies of '${this.name}' available`);
    this._availableCopies--;
  }
  checkIn(): void { this._availableCopies++; }
}
