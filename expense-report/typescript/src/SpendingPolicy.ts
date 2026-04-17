import { Category } from './Category.js';
import { Money } from './Money.js';

const perItemLimits: Record<Category, Money> = {
  [Category.Meals]: new Money(50),
  [Category.Travel]: new Money(500),
  [Category.Accommodation]: new Money(200),
  [Category.Equipment]: new Money(1000),
  [Category.Other]: new Money(100),
};

export const SpendingPolicy = {
  limitFor(category: Category): Money {
    return perItemLimits[category];
  },
  reportMaximum: new Money(5000),
  approvalThreshold: new Money(2500),
} as const;
