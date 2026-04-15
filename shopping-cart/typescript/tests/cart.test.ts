import { describe, expect, it } from 'vitest';
import { BulkPricing, BuyXGetY, FixedOff, PercentOff } from '../src/DiscountPolicy.js';
import { Money } from '../src/Money.js';
import { Quantity } from '../src/Quantity.js';
import { CartBuilder } from './CartBuilder.js';
import { ProductBuilder } from './ProductBuilder.js';

describe('Basic Cart Operations', () => {
  it('new cart is empty', () => {
    const cart = new CartBuilder().build();
    expect(cart.isEmpty).toBe(true);
    expect(cart.lines).toEqual([]);
  });

  it('adding a product adds one line item with quantity one', () => {
    const apple = new ProductBuilder().withSku('APPLE').named('Apple').pricedAt(1.0).build();
    const cart = new CartBuilder().build();
    cart.add(apple);
    expect(cart.lines).toHaveLength(1);
    expect(cart.lines[0]!.product).toBe(apple);
    expect(cart.lines[0]!.quantity.value).toBe(1);
  });

  it('adding the same product twice increments the existing lines quantity', () => {
    const apple = new ProductBuilder().withSku('APPLE').build();
    const cart = new CartBuilder().withProduct(apple).build();
    cart.add(apple);
    expect(cart.lines).toHaveLength(1);
    expect(cart.lines[0]!.quantity.value).toBe(2);
  });

  it('removing a product removes its line item', () => {
    const apple = new ProductBuilder().withSku('APPLE').build();
    const bread = new ProductBuilder().withSku('BREAD').build();
    const cart = new CartBuilder().withProduct(apple).withProduct(bread).build();
    cart.remove('APPLE');
    expect(cart.lines).toHaveLength(1);
    expect(cart.lines[0]!.product.sku).toBe('BREAD');
  });

  it('updating quantity to a positive number replaces the lines quantity', () => {
    const apple = new ProductBuilder().withSku('APPLE').build();
    const cart = new CartBuilder().withProduct(apple).build();
    cart.updateQuantity('APPLE', 5);
    expect(cart.lines[0]!.quantity.value).toBe(5);
  });

  it('updating quantity to zero is rejected', () => {
    const apple = new ProductBuilder().withSku('APPLE').build();
    const cart = new CartBuilder().withProduct(apple).build();
    expect(() => cart.updateQuantity('APPLE', 0)).toThrow(RangeError);
  });

  it('updating quantity to a negative number is rejected', () => {
    const apple = new ProductBuilder().withSku('APPLE').build();
    const cart = new CartBuilder().withProduct(apple).build();
    expect(() => cart.updateQuantity('APPLE', -1)).toThrow(RangeError);
  });
});

describe('Subtotals and Totals', () => {
  it('line subtotal is unit price multiplied by quantity', () => {
    const apple = new ProductBuilder().withSku('APPLE').pricedAt(1.25).build();
    const cart = new CartBuilder().withProduct(apple, new Quantity(4)).build();
    expect(cart.lines[0]!.subtotal().equals(new Money(5.0))).toBe(true);
  });

  it('cart total is the sum of line subtotals', () => {
    const apple = new ProductBuilder().withSku('APPLE').pricedAt(1.25).build();
    const bread = new ProductBuilder().withSku('BREAD').pricedAt(3.0).build();
    const cart = new CartBuilder()
      .withProduct(apple, new Quantity(4))
      .withProduct(bread, new Quantity(2))
      .build();
    expect(cart.total().equals(new Money(11.0))).toBe(true);
  });

  it('cart total of an empty cart is zero', () => {
    const cart = new CartBuilder().build();
    expect(cart.total().equals(Money.zero)).toBe(true);
  });
});

describe('Percent Discount', () => {
  it('ten percent off reduces the line subtotal by ten percent', () => {
    const apple = new ProductBuilder()
      .withSku('APPLE')
      .pricedAt(10.0)
      .withDiscount(new PercentOff(10))
      .build();
    const cart = new CartBuilder().withProduct(apple, new Quantity(3)).build();
    expect(cart.lines[0]!.subtotal().equals(new Money(27.0))).toBe(true);
  });

  it('percent discount applies before cart total is summed', () => {
    const apple = new ProductBuilder()
      .withSku('APPLE')
      .pricedAt(10.0)
      .withDiscount(new PercentOff(10))
      .build();
    const bread = new ProductBuilder().withSku('BREAD').pricedAt(5.0).build();
    const cart = new CartBuilder()
      .withProduct(apple, new Quantity(3))
      .withProduct(bread, new Quantity(2))
      .build();
    expect(cart.total().equals(new Money(37.0))).toBe(true);
  });
});

describe('Fixed Discount', () => {
  it('fixed discount subtracts a flat amount from the line subtotal', () => {
    const apple = new ProductBuilder()
      .withSku('APPLE')
      .pricedAt(10.0)
      .withDiscount(new FixedOff(new Money(2.5)))
      .build();
    const cart = new CartBuilder().withProduct(apple, new Quantity(3)).build();
    expect(cart.lines[0]!.subtotal().equals(new Money(27.5))).toBe(true);
  });

  it('fixed discount cannot take a line subtotal below zero', () => {
    const apple = new ProductBuilder()
      .withSku('APPLE')
      .pricedAt(2.0)
      .withDiscount(new FixedOff(new Money(10.0)))
      .build();
    const cart = new CartBuilder().withProduct(apple, new Quantity(1)).build();
    expect(cart.lines[0]!.subtotal().equals(Money.zero)).toBe(true);
  });
});

describe('Buy X Get Y Free', () => {
  it('buy two get one free charges only for two when three are bought', () => {
    const apple = new ProductBuilder()
      .withSku('APPLE')
      .pricedAt(3.0)
      .withDiscount(new BuyXGetY(2, 1))
      .build();
    const cart = new CartBuilder().withProduct(apple, new Quantity(3)).build();
    expect(cart.lines[0]!.subtotal().equals(new Money(6.0))).toBe(true);
  });

  it('buy two get one free charges for four when five are bought', () => {
    const apple = new ProductBuilder()
      .withSku('APPLE')
      .pricedAt(3.0)
      .withDiscount(new BuyXGetY(2, 1))
      .build();
    const cart = new CartBuilder().withProduct(apple, new Quantity(5)).build();
    expect(cart.lines[0]!.subtotal().equals(new Money(12.0))).toBe(true);
  });
});

describe('Bulk Pricing', () => {
  it('bulk pricing applies the lower unit price once the threshold is reached', () => {
    const apple = new ProductBuilder()
      .withSku('APPLE')
      .pricedAt(1.0)
      .withDiscount(new BulkPricing(new Quantity(3), new Money(0.75)))
      .build();
    const cart = new CartBuilder().withProduct(apple, new Quantity(4)).build();
    expect(cart.lines[0]!.subtotal().equals(new Money(3.0))).toBe(true);
  });

  it('bulk pricing does not apply below the threshold', () => {
    const apple = new ProductBuilder()
      .withSku('APPLE')
      .pricedAt(1.0)
      .withDiscount(new BulkPricing(new Quantity(3), new Money(0.75)))
      .build();
    const cart = new CartBuilder().withProduct(apple, new Quantity(2)).build();
    expect(cart.lines[0]!.subtotal().equals(new Money(2.0))).toBe(true);
  });
});
