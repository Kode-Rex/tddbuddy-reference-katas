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
});
