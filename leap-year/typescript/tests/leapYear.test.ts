import { describe, it, expect } from 'vitest';
import { isLeapYear } from '../src/leapYear.js';

describe('isLeapYear', () => {
  it('returns false for 2023 (not divisible by four)', () => {
    expect(isLeapYear(2023)).toBe(false);
  });

  it('returns true for 2024 (divisible by four, not by one hundred)', () => {
    expect(isLeapYear(2024)).toBe(true);
  });

  it('returns true for 2020 (another typical leap year)', () => {
    expect(isLeapYear(2020)).toBe(true);
  });

  it('returns false for 1900 (divisible by one hundred but not by four hundred)', () => {
    expect(isLeapYear(1900)).toBe(false);
  });

  it('returns false for 2100 (divisible by one hundred but not by four hundred)', () => {
    expect(isLeapYear(2100)).toBe(false);
  });

  it('returns true for 2000 (divisible by four hundred)', () => {
    expect(isLeapYear(2000)).toBe(true);
  });

  it('returns true for 1600 (another century divisible by four hundred)', () => {
    expect(isLeapYear(1600)).toBe(true);
  });

  it('returns false for 2001 (not divisible by four)', () => {
    expect(isLeapYear(2001)).toBe(false);
  });
});
