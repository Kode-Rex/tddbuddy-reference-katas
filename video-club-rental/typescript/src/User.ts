import { Age } from './Age.js';

export class User {
  private _priorityPoints = 0;
  private _loyaltyPoints = 0;
  private _hasOverdue = false;
  private _wishlist = new Set<string>();

  constructor(
    public readonly name: string,
    public readonly email: string,
    public readonly age: Age,
    public readonly isAdmin: boolean = false,
  ) {}

  get priorityPoints(): number { return this._priorityPoints; }
  get loyaltyPoints(): number { return this._loyaltyPoints; }
  get hasOverdue(): boolean { return this._hasOverdue; }
  get wishlist(): ReadonlySet<string> { return this._wishlist; }

  awardPriorityPoints(points: number): void { this._priorityPoints += points; }
  deductPriorityPoints(points: number): void {
    this._priorityPoints = Math.max(0, this._priorityPoints - points);
  }
  awardLoyaltyPoints(points: number): void { this._loyaltyPoints += points; }
  markOverdue(): void { this._hasOverdue = true; }
  clearOverdue(): void { this._hasOverdue = false; }

  addWish(titleName: string): void { this._wishlist.add(titleName.toLowerCase()); }
  wishes(titleName: string): boolean { return this._wishlist.has(titleName.toLowerCase()); }

  /** Test-only: seed priority points. */
  seedPriorityPoints(points: number): void { this._priorityPoints = points; }
}
