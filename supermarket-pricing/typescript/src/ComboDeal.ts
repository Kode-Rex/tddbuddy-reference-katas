import { Money } from './Money.js';

export class ComboDeal {
  constructor(
    public readonly skuA: string,
    public readonly skuB: string,
    public readonly dealPrice: Money,
  ) {}
}
