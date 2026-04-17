import { describe, expect, it } from 'vitest';
import { Checkout } from '../src/Checkout.js';
import { Money } from '../src/Money.js';
import { CheckoutBuilder } from './CheckoutBuilder.js';
import { ProductBuilder } from './ProductBuilder.js';

describe('Simple Pricing', () => {
  it('empty checkout has a zero total', () => {
    const checkout = new CheckoutBuilder().build();
    expect(checkout.total().equals(Money.zero)).toBe(true);
  });

  it('scanning a single item returns its unit price', () => {
    const a = new ProductBuilder().withSku('A').named('A').withUnitPrice(50).build();
    const checkout = new CheckoutBuilder().withScanned(a).build();
    expect(checkout.total().equals(new Money(50))).toBe(true);
  });

  it('scanning two different items returns the sum of their unit prices', () => {
    const a = new ProductBuilder().withSku('A').named('A').withUnitPrice(50).build();
    const b = new ProductBuilder().withSku('B').named('B').withUnitPrice(30).build();
    const checkout = new CheckoutBuilder().withScanned(a).withScanned(b).build();
    expect(checkout.total().equals(new Money(80))).toBe(true);
  });

  it('scanning the same item twice returns double its unit price', () => {
    const a = new ProductBuilder().withSku('A').named('A').withUnitPrice(50).build();
    const checkout = new CheckoutBuilder().withScanned(a, 2).build();
    expect(checkout.total().equals(new Money(100))).toBe(true);
  });
});

describe('Multi-Buy Discounts', () => {
  it('three As at three-for-130 costs 130', () => {
    const a = new ProductBuilder().withSku('A').named('A').withMultiBuy(3, 130, 50).build();
    const checkout = new CheckoutBuilder().withScanned(a, 3).build();
    expect(checkout.total().equals(new Money(130))).toBe(true);
  });

  it('four As at three-for-130 costs 180', () => {
    const a = new ProductBuilder().withSku('A').named('A').withMultiBuy(3, 130, 50).build();
    const checkout = new CheckoutBuilder().withScanned(a, 4).build();
    expect(checkout.total().equals(new Money(180))).toBe(true);
  });

  it('two Bs at two-for-45 costs 45', () => {
    const b = new ProductBuilder().withSku('B').named('B').withMultiBuy(2, 45, 30).build();
    const checkout = new CheckoutBuilder().withScanned(b, 2).build();
    expect(checkout.total().equals(new Money(45))).toBe(true);
  });

  it('three Bs at two-for-45 costs 75', () => {
    const b = new ProductBuilder().withSku('B').named('B').withMultiBuy(2, 45, 30).build();
    const checkout = new CheckoutBuilder().withScanned(b, 3).build();
    expect(checkout.total().equals(new Money(75))).toBe(true);
  });

  it('mixed basket with multi-buy discounts totals correctly', () => {
    const a = new ProductBuilder().withSku('A').named('A').withMultiBuy(3, 130, 50).build();
    const b = new ProductBuilder().withSku('B').named('B').withMultiBuy(2, 45, 30).build();
    const checkout = new CheckoutBuilder()
      .withScanned(a, 3)
      .withScanned(b, 2)
      .build();
    expect(checkout.total().equals(new Money(175))).toBe(true);
  });

  it('scanning order does not affect multi-buy total', () => {
    const a = new ProductBuilder().withSku('A').named('A').withMultiBuy(3, 130, 50).build();
    const b = new ProductBuilder().withSku('B').named('B').withMultiBuy(2, 45, 30).build();
    const checkout = new Checkout();
    checkout.scan(a);
    checkout.scan(b);
    checkout.scan(a);
    checkout.scan(b);
    checkout.scan(a);
    expect(checkout.total().equals(new Money(175))).toBe(true);
  });
});

describe('Buy One Get One Free', () => {
  it('two Cs with BOGOF costs 20', () => {
    const c = new ProductBuilder().withSku('C').named('C').withBuyOneGetOneFree(20).build();
    const checkout = new CheckoutBuilder().withScanned(c, 2).build();
    expect(checkout.total().equals(new Money(20))).toBe(true);
  });

  it('three Cs with BOGOF costs 40', () => {
    const c = new ProductBuilder().withSku('C').named('C').withBuyOneGetOneFree(20).build();
    const checkout = new CheckoutBuilder().withScanned(c, 3).build();
    expect(checkout.total().equals(new Money(40))).toBe(true);
  });

  it('single C with BOGOF costs 20', () => {
    const c = new ProductBuilder().withSku('C').named('C').withBuyOneGetOneFree(20).build();
    const checkout = new CheckoutBuilder().withScanned(c, 1).build();
    expect(checkout.total().equals(new Money(20))).toBe(true);
  });
});

describe('Weighted Items', () => {
  it('bananas at 199 cents per kg for half kg costs 100', () => {
    const bananas = new ProductBuilder().withSku('Bananas').named('Bananas').withWeightedPrice(199).build();
    const checkout = new CheckoutBuilder().withWeighed(bananas, 0.5).build();
    expect(checkout.total().equals(new Money(100))).toBe(true);
  });

  it('apples at 349 cents per kg for one kg costs 349', () => {
    const apples = new ProductBuilder().withSku('Apples').named('Apples').withWeightedPrice(349).build();
    const checkout = new CheckoutBuilder().withWeighed(apples, 1.0).build();
    expect(checkout.total().equals(new Money(349))).toBe(true);
  });

  it('weighted item with zero weight costs zero', () => {
    const bananas = new ProductBuilder().withSku('Bananas').named('Bananas').withWeightedPrice(199).build();
    const checkout = new CheckoutBuilder().withWeighed(bananas, 0).build();
    expect(checkout.total().equals(Money.zero)).toBe(true);
  });
});

describe('Combo Deals', () => {
  it('D plus C together at combo price costs 25', () => {
    const d = new ProductBuilder().withSku('D').named('D').withUnitPrice(15).build();
    const c = new ProductBuilder().withSku('C').named('C').withUnitPrice(20).build();
    const checkout = new CheckoutBuilder()
      .withComboDeal('D', 'C', 25)
      .withScanned(d)
      .withScanned(c)
      .build();
    expect(checkout.total().equals(new Money(25))).toBe(true);
  });

  it('D plus C plus D uses combo once, remaining D at unit price', () => {
    const d = new ProductBuilder().withSku('D').named('D').withUnitPrice(15).build();
    const c = new ProductBuilder().withSku('C').named('C').withUnitPrice(20).build();
    const checkout = new CheckoutBuilder()
      .withComboDeal('D', 'C', 25)
      .withScanned(d, 2)
      .withScanned(c)
      .build();
    expect(checkout.total().equals(new Money(40))).toBe(true);
  });

  it('D alone with a combo deal configured still costs unit price', () => {
    const d = new ProductBuilder().withSku('D').named('D').withUnitPrice(15).build();
    const checkout = new CheckoutBuilder()
      .withComboDeal('D', 'C', 25)
      .withScanned(d)
      .build();
    expect(checkout.total().equals(new Money(15))).toBe(true);
  });

  it('C alone with a combo deal configured still costs unit price', () => {
    const c = new ProductBuilder().withSku('C').named('C').withUnitPrice(20).build();
    const checkout = new CheckoutBuilder()
      .withComboDeal('D', 'C', 25)
      .withScanned(c)
      .build();
    expect(checkout.total().equals(new Money(20))).toBe(true);
  });
});
