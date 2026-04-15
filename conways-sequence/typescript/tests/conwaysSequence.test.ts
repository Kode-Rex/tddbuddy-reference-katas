import { describe, it, expect } from 'vitest';
import { nextTerm, lookAndSay } from '../src/conwaysSequence.js';

describe('nextTerm', () => {
  it('"1" has one 1 → "11"', () => {
    expect(nextTerm('1')).toBe('11');
  });

  it('"11" has two 1s → "21"', () => {
    expect(nextTerm('11')).toBe('21');
  });

  it('"21" has one 2 and one 1 → "1211"', () => {
    expect(nextTerm('21')).toBe('1211');
  });

  it('"1211" has one 1, one 2, two 1s → "111221"', () => {
    expect(nextTerm('1211')).toBe('111221');
  });

  it('"111221" has three 1s, two 2s, one 1 → "312211"', () => {
    expect(nextTerm('111221')).toBe('312211');
  });

  it('"2" alone is one 2 → "12"', () => {
    expect(nextTerm('2')).toBe('12');
  });

  it('"22" is two 2s → "22" (a fixed point)', () => {
    expect(nextTerm('22')).toBe('22');
  });

  it('"3211" has one 3, one 2, two 1s → "131221"', () => {
    expect(nextTerm('3211')).toBe('131221');
  });

  it('ten consecutive 1s are described as ten 1s → "101"', () => {
    expect(nextTerm('1111111111')).toBe('101');
  });
});

describe('lookAndSay', () => {
  it('zero iterations returns the seed unchanged', () => {
    expect(lookAndSay('1', 0)).toBe('1');
  });

  it('one iteration equals a single nextTerm', () => {
    expect(lookAndSay('1', 1)).toBe('11');
  });

  it('five iterations from "1" land on "312211"', () => {
    expect(lookAndSay('1', 5)).toBe('312211');
  });

  it('two iterations from seed "2" is "1112"', () => {
    expect(lookAndSay('2', 2)).toBe('1112');
  });

  it('negative iteration count is rejected', () => {
    expect(() => lookAndSay('1', -1)).toThrow('iterations must be non-negative');
  });
});
