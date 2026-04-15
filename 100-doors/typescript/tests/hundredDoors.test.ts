import { describe, it, expect } from 'vitest';
import { openDoors } from '../src/hundredDoors.js';

describe('openDoors', () => {
  it('returns an empty list when there are zero doors', () => {
    expect(openDoors(0)).toEqual([]);
  });

  it('returns [1] when there is one door (pass one opens it)', () => {
    expect(openDoors(1)).toEqual([1]);
  });

  it('returns [1, 4, 9] for ten doors — the perfect squares up to ten', () => {
    expect(openDoors(10)).toEqual([1, 4, 9]);
  });

  it('returns the ten perfect squares for one hundred doors', () => {
    expect(openDoors(100)).toEqual([1, 4, 9, 16, 25, 36, 49, 64, 81, 100]);
  });
});
