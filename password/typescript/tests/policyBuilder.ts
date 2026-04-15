import { createPolicy, DEFAULT_MIN_LENGTH, type Policy } from '../src/policy.js';

export class PolicyBuilder {
  private _minLength = DEFAULT_MIN_LENGTH;
  private _requiresDigit = false;
  private _requiresSymbol = false;
  private _requiresUpper = false;
  private _requiresLower = false;

  minLength(n: number): this { this._minLength = n; return this; }
  requiresDigit(): this { this._requiresDigit = true; return this; }
  requiresSymbol(): this { this._requiresSymbol = true; return this; }
  requiresUpper(): this { this._requiresUpper = true; return this; }
  requiresLower(): this { this._requiresLower = true; return this; }

  build(): Policy {
    return createPolicy({
      minLength: this._minLength,
      requiresDigit: this._requiresDigit,
      requiresSymbol: this._requiresSymbol,
      requiresUpper: this._requiresUpper,
      requiresLower: this._requiresLower,
    });
  }
}
