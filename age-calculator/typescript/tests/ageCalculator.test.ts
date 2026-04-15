import { describe, it, expect } from 'vitest';
import { calculate } from '../src/ageCalculator.js';

const utc = (year: number, month: number, day: number): Date =>
  new Date(Date.UTC(year, month - 1, day));

describe('calculate (age from birthdate + reference date)', () => {
  it('Zenith born 2016-10-28 on 2022-11-05 is 6', () => {
    expect(calculate(utc(2016, 10, 28), utc(2022, 11, 5))).toBe(6);
  });

  it('Zenith on her seventh birthday is 7', () => {
    expect(calculate(utc(2016, 10, 28), utc(2023, 10, 28))).toBe(7);
  });

  it('Zenith on the day before her seventh birthday is 6', () => {
    expect(calculate(utc(2016, 10, 28), utc(2023, 10, 27))).toBe(6);
  });

  it('born 2000-01-01 on 2024-12-31 is 24', () => {
    expect(calculate(utc(2000, 1, 1), utc(2024, 12, 31))).toBe(24);
  });

  it('born today is 0', () => {
    expect(calculate(utc(2000, 1, 1), utc(2000, 1, 1))).toBe(0);
  });

  it('leap-day baby on February 28 in a non-leap year has not yet aged up', () => {
    expect(calculate(utc(2000, 2, 29), utc(2001, 2, 28))).toBe(0);
  });

  it('leap-day baby ages up on March 1 in a non-leap year', () => {
    expect(calculate(utc(2000, 2, 29), utc(2001, 3, 1))).toBe(1);
  });

  it('leap-day baby on an actual leap day ages up exactly', () => {
    expect(calculate(utc(2000, 2, 29), utc(2004, 2, 29))).toBe(4);
  });

  it('born yesterday across a year boundary is still 0', () => {
    expect(calculate(utc(1999, 12, 31), utc(2000, 1, 1))).toBe(0);
  });

  it('born 1990-06-15 on 2024-06-14 is 33', () => {
    expect(calculate(utc(1990, 6, 15), utc(2024, 6, 14))).toBe(33);
  });

  it('throws when birthdate is after today', () => {
    expect(() => calculate(utc(2024, 6, 15), utc(2024, 6, 14)))
      .toThrow('birthdate is after today');
  });
});
