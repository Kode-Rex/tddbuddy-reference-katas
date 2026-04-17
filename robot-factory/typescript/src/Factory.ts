import type { CostBreakdown } from './CostBreakdown.js';
import { Money } from './Money.js';
import { PartNotAvailableError } from './PartNotAvailableError.js';
import type { PartQuote } from './PartQuote.js';
import type { PartSupplier } from './PartSupplier.js';
import type { PurchasedPart } from './PurchasedPart.js';
import type { RobotOrder } from './RobotOrder.js';

export class Factory {
  private readonly suppliers: PartSupplier[];

  constructor(suppliers: PartSupplier[]) {
    this.suppliers = [...suppliers];
  }

  costRobot(order: RobotOrder): CostBreakdown {
    order.validate();

    const cheapestParts: PartQuote[] = [];

    for (const [type, option] of order.parts) {
      const quotes = this.suppliers
        .map(s => s.getQuote(type, option))
        .filter((q): q is PartQuote => q !== null);

      if (quotes.length === 0) {
        throw new PartNotAvailableError(`Part not available: ${option}`);
      }

      const cheapest = quotes.reduce((best, q) =>
        q.price.lessThan(best.price) ? q : best);
      cheapestParts.push(cheapest);
    }

    const total = cheapestParts.reduce(
      (sum, q) => sum.plus(q.price), Money.zero);

    return { parts: cheapestParts, total };
  }

  purchaseRobot(order: RobotOrder): PurchasedPart[] {
    const breakdown = this.costRobot(order);

    return breakdown.parts.map(quote => {
      const supplier = this.suppliers.find(s => s.name === quote.supplierName)!;
      return supplier.purchase(quote.type, quote.option);
    });
  }
}
