import { Money } from './Money.js';
import { Product } from './Product.js';
import {
  InsufficientPaymentException,
  OutOfStockException,
  UnknownPurchaseCodeException,
} from './Exceptions.js';

export class BakeSale {
  constructor(private readonly _inventory: Product[]) {}

  get inventory(): readonly Product[] {
    return this._inventory;
  }

  static createDefault(): BakeSale {
    return new BakeSale([
      new Product('Brownie', new Money(0.75), 'B', 48),
      new Product('Muffin', new Money(1.00), 'M', 36),
      new Product('Cake Pop', new Money(1.35), 'C', 24),
      new Product('Water', new Money(1.50), 'W', 30),
    ]);
  }

  calculateTotal(order: string): Money {
    const codes = order.split(',').map((c) => c.trim());
    const products = this.resolveProducts(codes);

    this.validateStock(products);

    let total = Money.zero;
    for (const product of products) {
      total = total.plus(product.price);
    }

    for (const product of products) {
      product.decrementStock();
    }

    return total;
  }

  calculateChange(total: Money, payment: Money): Money {
    if (payment.lessThan(total)) {
      throw new InsufficientPaymentException();
    }
    return payment.minus(total);
  }

  private resolveProducts(codes: string[]): Product[] {
    const products: Product[] = [];
    for (const code of codes) {
      const product = this._inventory.find((p) => p.purchaseCode === code);
      if (!product) {
        throw new UnknownPurchaseCodeException(code);
      }
      products.push(product);
    }
    return products;
  }

  private validateStock(products: Product[]): void {
    const grouped = new Map<string, { product: Product; count: number }>();
    for (const product of products) {
      const existing = grouped.get(product.purchaseCode);
      if (existing) {
        existing.count++;
      } else {
        grouped.set(product.purchaseCode, { product, count: 1 });
      }
    }
    for (const { product, count } of grouped.values()) {
      if (product.stock < count) {
        throw new OutOfStockException(product.name);
      }
    }
  }
}
