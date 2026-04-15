import { describe, it, expect } from 'vitest';
import { makeChange } from '../src/changeMaker.js';

const usCoins = [25, 10, 5, 1] as const;
const ukCoins = [50, 20, 10, 5, 2, 1] as const;
const norwayCoins = [20, 10, 5, 1] as const;

describe('makeChange', () => {
  it('returns no coins for a zero amount', () => {
    expect(makeChange(0, usCoins)).toEqual([]);
  });

  it('US: 1 cent is a single penny', () => {
    expect(makeChange(1, usCoins)).toEqual([1]);
  });

  it('US: 5 cents is a single nickel', () => {
    expect(makeChange(5, usCoins)).toEqual([5]);
  });

  it('US: 25 cents is a single quarter', () => {
    expect(makeChange(25, usCoins)).toEqual([25]);
  });

  it('US: 30 cents is a quarter and a nickel', () => {
    expect(makeChange(30, usCoins)).toEqual([25, 5]);
  });

  it('US: 41 cents is a quarter, a dime, a nickel, and a penny', () => {
    expect(makeChange(41, usCoins)).toEqual([25, 10, 5, 1]);
  });

  it('US: 66 cents is two quarters, a dime, a nickel, and a penny', () => {
    expect(makeChange(66, usCoins)).toEqual([25, 25, 10, 5, 1]);
  });

  it('UK: 43 pence is 20, 20, 2, 1', () => {
    expect(makeChange(43, ukCoins)).toEqual([20, 20, 2, 1]);
  });

  it('UK: 88 pence is one of each British coin', () => {
    expect(makeChange(88, ukCoins)).toEqual([50, 20, 10, 5, 2, 1]);
  });

  it('Norway: 37 ore is 20, 10, 5, 1, 1', () => {
    expect(makeChange(37, norwayCoins)).toEqual([20, 10, 5, 1, 1]);
  });

  it('Norway: 40 ore is two 20-ore coins', () => {
    expect(makeChange(40, norwayCoins)).toEqual([20, 20]);
  });
});
