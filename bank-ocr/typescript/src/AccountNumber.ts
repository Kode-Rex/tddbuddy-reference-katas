import { AccountLength } from './constants.js';
import { Digit } from './Digit.js';

export class AccountNumber {
  readonly digits: readonly Digit[];

  constructor(digits: Digit[]) {
    if (digits.length !== AccountLength) {
      throw new Error(`Account number must have exactly ${AccountLength} digits.`);
    }
    this.digits = [...digits];
  }

  get isLegible(): boolean {
    return this.digits.every((d) => d.isKnown);
  }

  get isChecksumValid(): boolean {
    if (!this.isLegible) return false;
    let sum = 0;
    for (let i = 0; i < AccountLength; i++) {
      const position = AccountLength - i; // d1 -> 9, d9 -> 1
      sum += (this.digits[i]!.value as number) * position;
    }
    return sum % 11 === 0;
  }

  get number(): string {
    return this.digits.map((d) => d.toString()).join('');
  }

  get status(): string {
    if (!this.isLegible) return `${this.number} ILL`;
    if (!this.isChecksumValid) return `${this.number} ERR`;
    return this.number;
  }
}
