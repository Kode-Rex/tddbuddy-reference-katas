import { describe, it, expect } from 'vitest';
import { toRoman } from '../src/romanNumerals';

describe('toRoman', () => {
  it('1 is I', () => {
    expect(toRoman(1)).toBe('I');
  });

  it('2 is II', () => {
    expect(toRoman(2)).toBe('II');
  });

  it('3 is III', () => {
    expect(toRoman(3)).toBe('III');
  });

  it('5 is V', () => {
    expect(toRoman(5)).toBe('V');
  });

  it('4 is IV', () => {
    expect(toRoman(4)).toBe('IV');
  });

  it('10 is X', () => {
    expect(toRoman(10)).toBe('X');
  });

  it('9 is IX', () => {
    expect(toRoman(9)).toBe('IX');
  });

  it('40 is XL', () => {
    expect(toRoman(40)).toBe('XL');
  });

  it('90 is XC', () => {
    expect(toRoman(90)).toBe('XC');
  });

  it('400 is CD', () => {
    expect(toRoman(400)).toBe('CD');
  });

  it('900 is CM', () => {
    expect(toRoman(900)).toBe('CM');
  });

  it('1000 is M', () => {
    expect(toRoman(1000)).toBe('M');
  });

  it('1984 is MCMLXXXIV', () => {
    expect(toRoman(1984)).toBe('MCMLXXXIV');
  });

  it('3999 is MMMCMXCIX', () => {
    expect(toRoman(3999)).toBe('MMMCMXCIX');
  });
});
