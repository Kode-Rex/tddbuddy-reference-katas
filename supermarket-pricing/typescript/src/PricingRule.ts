import { Money } from './Money.js';
import { Weight } from './Weight.js';

export interface PricingRule {
  calculate(quantity: number, weight: Weight): Money;
}

export class UnitPrice implements PricingRule {
  constructor(public readonly price: Money) {}

  calculate(quantity: number, _weight: Weight): Money {
    return this.price.times(quantity);
  }
}

export class MultiBuy implements PricingRule {
  constructor(
    public readonly groupSize: number,
    public readonly groupPrice: Money,
    public readonly itemPrice: Money,
  ) {
    if (groupSize <= 0) {
      throw new RangeError('Group size must be positive');
    }
  }

  calculate(quantity: number, _weight: Weight): Money {
    const fullGroups = Math.floor(quantity / this.groupSize);
    const remainder = quantity % this.groupSize;
    return this.groupPrice.times(fullGroups).plus(this.itemPrice.times(remainder));
  }
}

export class BuyOneGetOneFree implements PricingRule {
  constructor(public readonly itemPrice: Money) {}

  calculate(quantity: number, _weight: Weight): Money {
    const chargeable = Math.ceil(quantity / 2);
    return this.itemPrice.times(chargeable);
  }
}

export class WeightedPrice implements PricingRule {
  constructor(public readonly centsPerKg: number) {
    if (centsPerKg < 0) {
      throw new RangeError('Price per kg must not be negative');
    }
  }

  calculate(_quantity: number, weight: Weight): Money {
    const rawCents = weight.kg * this.centsPerKg;
    return new Money(Math.round(rawCents));
  }
}
