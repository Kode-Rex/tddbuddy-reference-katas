import { Money } from './Money.js';
import { Quantity } from './Quantity.js';

export interface DiscountPolicy {
  apply(unitPrice: Money, quantity: Quantity): Money;
}

export class NoDiscount implements DiscountPolicy {
  static readonly instance = new NoDiscount();
  apply(unitPrice: Money, quantity: Quantity): Money {
    return unitPrice.times(quantity.value);
  }
}

const MIN_PERCENT = 0;
const MAX_PERCENT = 100;
const HUNDRED_PERCENT = 100;

export class PercentOff implements DiscountPolicy {
  constructor(public readonly percent: number) {
    if (!Number.isInteger(percent) || percent < MIN_PERCENT || percent > MAX_PERCENT) {
      throw new RangeError('Percent must be an integer between 0 and 100');
    }
  }

  apply(unitPrice: Money, quantity: Quantity): Money {
    const multiplier = (HUNDRED_PERCENT - this.percent) / HUNDRED_PERCENT;
    return new Money(unitPrice.amount * quantity.value * multiplier);
  }
}

export class FixedOff implements DiscountPolicy {
  constructor(public readonly amount: Money) {
    if (amount.lessThan(Money.zero)) {
      throw new RangeError('Fixed discount amount must not be negative');
    }
  }

  apply(unitPrice: Money, quantity: Quantity): Money {
    const gross = unitPrice.times(quantity.value);
    const discounted = gross.minus(this.amount);
    return discounted.lessThan(Money.zero) ? Money.zero : discounted;
  }
}

export class BuyXGetY implements DiscountPolicy {
  constructor(public readonly buyCount: number, public readonly freeCount: number) {
    if (!Number.isInteger(buyCount) || buyCount <= 0) {
      throw new RangeError('Buy count must be a positive integer');
    }
    if (!Number.isInteger(freeCount) || freeCount <= 0) {
      throw new RangeError('Free count must be a positive integer');
    }
  }

  apply(unitPrice: Money, quantity: Quantity): Money {
    const groupSize = this.buyCount + this.freeCount;
    const fullGroups = Math.floor(quantity.value / groupSize);
    const remainder = quantity.value % groupSize;
    const chargeable = fullGroups * this.buyCount + Math.min(remainder, this.buyCount);
    return unitPrice.times(chargeable);
  }
}

export class BulkPricing implements DiscountPolicy {
  constructor(public readonly threshold: Quantity, public readonly bulkUnitPrice: Money) {
    if (bulkUnitPrice.lessThan(Money.zero)) {
      throw new RangeError('Bulk unit price must not be negative');
    }
  }

  apply(unitPrice: Money, quantity: Quantity): Money {
    const effectiveUnitPrice = quantity.value >= this.threshold.value ? this.bulkUnitPrice : unitPrice;
    return effectiveUnitPrice.times(quantity.value);
  }
}
