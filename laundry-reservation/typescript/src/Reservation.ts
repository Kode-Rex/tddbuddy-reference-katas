import type { Customer } from './Customer.js';

export class Reservation {
  private _isUsed = false;

  constructor(
    public readonly id: string,
    public readonly slot: Date,
    public readonly machineNumber: number,
    public readonly customer: Customer,
    private _pin: number,
  ) {}

  get pin(): number { return this._pin; }
  get isUsed(): boolean { return this._isUsed; }

  markUsed(): void { this._isUsed = true; }
  updatePin(newPin: number): void { this._pin = newPin; }
}
