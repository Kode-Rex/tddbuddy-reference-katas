import { describe, it, expect } from 'vitest';
import { scale } from '../src/recipeCalculator.js';

describe('scale', () => {
  it('empty recipe scales to empty recipe', () => {
    expect(scale({}, 2)).toEqual({});
  });

  it('single ingredient doubles when factor is 2', () => {
    expect(scale({ flour: 100 }, 2)).toEqual({ flour: 200 });
  });

  it('single ingredient halves when factor is 0.5', () => {
    expect(scale({ flour: 100 }, 0.5)).toEqual({ flour: 50 });
  });

  it('multiple ingredients all scale by the same factor', () => {
    expect(scale({ flour: 200, sugar: 100, butter: 50 }, 3)).toEqual({
      flour: 600,
      sugar: 300,
      butter: 150,
    });
  });

  it('factor of 1 returns identical quantities', () => {
    expect(scale({ flour: 100, sugar: 50 }, 1)).toEqual({ flour: 100, sugar: 50 });
  });

  it('factor of 0 zeroes every quantity', () => {
    expect(scale({ flour: 100, sugar: 50 }, 0)).toEqual({ flour: 0, sugar: 0 });
  });

  it('fractional quantities scale without being rounded', () => {
    expect(scale({ salt: 1.5 }, 3)).toEqual({ salt: 4.5 });
  });

  it('negative factor is rejected', () => {
    expect(() => scale({ flour: 100 }, -1)).toThrow(RangeError);
  });
});
