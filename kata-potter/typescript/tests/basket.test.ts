import { describe, it, expect } from 'vitest';
import { BookOutOfRangeError } from '../src/basket.js';
import { BasketBuilder } from './basketBuilder.js';

const TOL = 0.001;

describe('Basket', () => {
  it('empty basket costs zero', () => {
    const basket = new BasketBuilder().build();
    expect(basket.price()).toBeCloseTo(0.0, 3);
  });

  it('one book costs the base price', () => {
    const basket = new BasketBuilder().withBook(1, 1).build();
    expect(basket.price()).toBeCloseTo(8.0, 3);
  });

  it('two copies of the same book get no discount', () => {
    const basket = new BasketBuilder().withBook(1, 2).build();
    expect(basket.price()).toBeCloseTo(16.0, 3);
  });

  it('two distinct books get the 5% discount', () => {
    const basket = new BasketBuilder()
      .withBook(1, 1)
      .withBook(2, 1)
      .build();
    expect(basket.price()).toBeCloseTo(15.2, 3);
  });

  it('three distinct books get the 10% discount', () => {
    const basket = new BasketBuilder()
      .withBook(1, 1).withBook(2, 1).withBook(3, 1)
      .build();
    expect(basket.price()).toBeCloseTo(21.6, 3);
  });

  it('four distinct books get the 20% discount', () => {
    const basket = new BasketBuilder()
      .withBook(1, 1).withBook(2, 1).withBook(3, 1).withBook(4, 1)
      .build();
    expect(basket.price()).toBeCloseTo(25.6, 3);
  });

  it('five distinct books get the 25% discount', () => {
    const basket = new BasketBuilder()
      .withBook(1, 1).withBook(2, 1).withBook(3, 1).withBook(4, 1).withBook(5, 1)
      .build();
    expect(basket.price()).toBeCloseTo(30.0, 3);
  });

  it('duplicates are priced separately from the discounted set', () => {
    const basket = new BasketBuilder()
      .withBook(1, 2)
      .withBook(2, 1)
      .build();
    // one 2-set (€15.20) + one 1-set (€8.00)
    expect(basket.price()).toBeCloseTo(23.2, 3);
  });

  it('two copies of every book makes two 5-sets', () => {
    const basket = new BasketBuilder()
      .withBook(1, 2).withBook(2, 2).withBook(3, 2).withBook(4, 2).withBook(5, 2)
      .build();
    expect(basket.price()).toBeCloseTo(60.0, 3);
  });

  it('greedy-fails basket prefers two 4-sets over a 5+3 pair', () => {
    // two each of books 1,2,3 plus one each of 4,5
    const basket = new BasketBuilder()
      .withBook(1, 2).withBook(2, 2).withBook(3, 2)
      .withBook(4, 1).withBook(5, 1)
      .build();
    // Two 4-sets (€25.60 each) beats a 5-set (€30.00) + 3-set (€21.60).
    expect(basket.price()).toBeCloseTo(51.2, 3);
  });

  it('bigger greedy-fails basket', () => {
    // three each of 1,2,3 plus two each of 4,5
    const basket = new BasketBuilder()
      .withBook(1, 3).withBook(2, 3).withBook(3, 3)
      .withBook(4, 2).withBook(5, 2)
      .build();
    // One 5-set + two 4-sets = €81.20; greedy's two 5-sets + 3-set = €81.60.
    expect(basket.price()).toBeCloseTo(81.2, 3);
  });

  it('BasketBuilder rejects book ids outside 1..5', () => {
    expect(() => new BasketBuilder().withBook(0, 1)).toThrow(BookOutOfRangeError);
    expect(() => new BasketBuilder().withBook(0, 1)).toThrow('book id must be between 1 and 5');
    expect(() => new BasketBuilder().withBook(6, 1)).toThrow(BookOutOfRangeError);
    expect(() => new BasketBuilder().withBook(6, 1)).toThrow('book id must be between 1 and 5');
  });
});

// Keep TOL referenced so unused-var linting doesn't trip on a tolerance constant
// that future assertions may use instead of toBeCloseTo.
void TOL;
