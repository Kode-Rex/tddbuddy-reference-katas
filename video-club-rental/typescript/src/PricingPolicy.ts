import { Money } from './Money.js';

export const BASE_PRICE = new Money(2.50);
export const SECOND_PRICE = new Money(2.25);
export const THIRD_PRICE = new Money(1.75);

export function calculate(newRentalCount: number, existingRentalCount: number): Money {
  let total = Money.zero;
  for (let i = 0; i < newRentalCount; i++) {
    const ordinal = existingRentalCount + i + 1;
    total = total.plus(priceForOrdinal(ordinal));
  }
  return total;
}

function priceForOrdinal(ordinal: number): Money {
  if (ordinal === 1) return BASE_PRICE;
  if (ordinal === 2) return SECOND_PRICE;
  return THIRD_PRICE;
}
