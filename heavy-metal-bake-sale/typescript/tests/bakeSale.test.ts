import { describe, it, expect } from 'vitest';
import { Money } from '../src/Money.js';
import { BakeSale } from '../src/BakeSale.js';
import {
  OutOfStockException,
  InsufficientPaymentException,
  UnknownPurchaseCodeException,
} from '../src/Exceptions.js';
import { ProductBuilder } from './ProductBuilder.js';
import { OrderBuilder } from './OrderBuilder.js';

describe('BakeSale', () => {
  // --- Product Setup ---

  it('a product has a name, price, purchase code, and stock quantity', () => {
    const product = new ProductBuilder()
      .withName('Brownie')
      .withPrice(0.75)
      .withPurchaseCode('B')
      .withStock(48)
      .build();

    expect(product.name).toBe('Brownie');
    expect(product.price.amount).toBe(0.75);
    expect(product.purchaseCode).toBe('B');
    expect(product.stock).toBe(48);
  });

  it('default inventory contains brownie, muffin, cake pop, and water', () => {
    const sale = BakeSale.createDefault();

    expect(sale.inventory).toHaveLength(4);
    expect(sale.inventory.map((p) => p.name)).toEqual([
      'Brownie', 'Muffin', 'Cake Pop', 'Water',
    ]);
  });

  // --- Order Totals ---

  it('single brownie order totals $0.75', () => {
    const sale = new OrderBuilder().withDefaultInventory().build();
    expect(sale.calculateTotal('B').amount).toBe(0.75);
  });

  it('single muffin order totals $1.00', () => {
    const sale = new OrderBuilder().withDefaultInventory().build();
    expect(sale.calculateTotal('M').amount).toBe(1.00);
  });

  it('single cake pop order totals $1.35', () => {
    const sale = new OrderBuilder().withDefaultInventory().build();
    expect(sale.calculateTotal('C').amount).toBe(1.35);
  });

  it('single water order totals $1.50', () => {
    const sale = new OrderBuilder().withDefaultInventory().build();
    expect(sale.calculateTotal('W').amount).toBe(1.50);
  });

  it('multiple different items total to the sum of their prices', () => {
    const sale = new OrderBuilder().withDefaultInventory().build();
    expect(sale.calculateTotal('B,C,W').amount).toBe(3.60);
  });

  it('duplicate items in an order are each counted separately', () => {
    const sale = new OrderBuilder().withDefaultInventory().build();
    expect(sale.calculateTotal('B,B').amount).toBe(1.50);
  });

  // --- Stock Management ---

  it('successful order decrements stock for each purchased item', () => {
    const sale = new OrderBuilder().withDefaultInventory().build();
    sale.calculateTotal('B,M');

    expect(sale.inventory.find((p) => p.purchaseCode === 'B')!.stock).toBe(47);
    expect(sale.inventory.find((p) => p.purchaseCode === 'M')!.stock).toBe(35);
  });

  it('out of stock item rejects the order with item name', () => {
    const sale = new OrderBuilder()
      .withProduct(new ProductBuilder().withName('Water').withPurchaseCode('W').withPrice(1.50).withStock(0).build())
      .build();

    expect(() => sale.calculateTotal('W')).toThrow(OutOfStockException);
    expect(() => sale.calculateTotal('W')).toThrow('Water is out of stock');
  });

  it('partially stocked order where second item is out of stock rejects with that item name', () => {
    const sale = new OrderBuilder()
      .withProduct(new ProductBuilder().withName('Brownie').withPurchaseCode('B').withStock(10).build())
      .withProduct(new ProductBuilder().withName('Water').withPurchaseCode('W').withPrice(1.50).withStock(0).build())
      .build();

    expect(() => sale.calculateTotal('B,W')).toThrow(OutOfStockException);
    expect(() => sale.calculateTotal('B,W')).toThrow('Water is out of stock');
  });

  it('order does not decrement stock when any item is out of stock', () => {
    const sale = new OrderBuilder()
      .withProduct(new ProductBuilder().withName('Brownie').withPurchaseCode('B').withStock(5).build())
      .withProduct(new ProductBuilder().withName('Water').withPurchaseCode('W').withPrice(1.50).withStock(0).build())
      .build();

    expect(() => sale.calculateTotal('B,W')).toThrow(OutOfStockException);
    expect(sale.inventory.find((p) => p.purchaseCode === 'B')!.stock).toBe(5);
  });

  // --- Payment and Change ---

  it('exact payment returns zero change', () => {
    const sale = new OrderBuilder().withDefaultInventory().build();
    const total = sale.calculateTotal('B');
    expect(sale.calculateChange(total, new Money(0.75)).amount).toBe(0);
  });

  it('overpayment returns correct change', () => {
    const sale = new OrderBuilder().withDefaultInventory().build();
    const total = sale.calculateTotal('B,C,W');
    expect(sale.calculateChange(total, new Money(4.00)).amount).toBe(0.40);
  });

  it('underpayment is rejected with not enough money', () => {
    const sale = new OrderBuilder().withDefaultInventory().build();
    const total = sale.calculateTotal('C,M');

    expect(() => sale.calculateChange(total, new Money(2.00))).toThrow(InsufficientPaymentException);
    expect(() => sale.calculateChange(total, new Money(2.00))).toThrow('Not enough money.');
  });

  // --- Edge Cases ---

  it('unknown purchase code is rejected', () => {
    const sale = new OrderBuilder().withDefaultInventory().build();

    expect(() => sale.calculateTotal('X')).toThrow(UnknownPurchaseCodeException);
    expect(() => sale.calculateTotal('X')).toThrow('Unknown purchase code: X');
  });

  it('multiple items with one out of stock reports the out of stock item', () => {
    const sale = new OrderBuilder()
      .withProduct(new ProductBuilder().withName('Brownie').withPurchaseCode('B').withStock(10).build())
      .withProduct(new ProductBuilder().withName('Muffin').withPurchaseCode('M').withPrice(1.00).withStock(0).build())
      .build();

    expect(() => sale.calculateTotal('B,M')).toThrow(OutOfStockException);
    expect(() => sale.calculateTotal('B,M')).toThrow('Muffin is out of stock');
  });

  it('buying all remaining stock of an item succeeds', () => {
    const sale = new OrderBuilder()
      .withProduct(new ProductBuilder().withName('Brownie').withPurchaseCode('B').withStock(1).build())
      .build();

    expect(sale.calculateTotal('B').amount).toBe(0.75);
    expect(sale.inventory.find((p) => p.purchaseCode === 'B')!.stock).toBe(0);
  });

  it('buying one more after stock is depleted fails', () => {
    const sale = new OrderBuilder()
      .withProduct(new ProductBuilder().withName('Brownie').withPurchaseCode('B').withStock(1).build())
      .build();

    sale.calculateTotal('B');
    expect(() => sale.calculateTotal('B')).toThrow(OutOfStockException);
    expect(() => sale.calculateTotal('B')).toThrow('Brownie is out of stock');
  });
});
