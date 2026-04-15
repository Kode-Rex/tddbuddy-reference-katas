import { describe, it, expect } from 'vitest';
import { toWords } from '../src/numbersToWords.js';

describe('toWords', () => {
  it('zero is spelled out', () => {
    expect(toWords(0)).toBe('zero');
  });

  it('five is spelled out', () => {
    expect(toWords(5)).toBe('five');
  });

  it('eight is spelled out', () => {
    expect(toWords(8)).toBe('eight');
  });

  it('ten is spelled out', () => {
    expect(toWords(10)).toBe('ten');
  });

  it('nineteen is a single word', () => {
    expect(toWords(19)).toBe('nineteen');
  });

  it('twenty is spelled out', () => {
    expect(toWords(20)).toBe('twenty');
  });

  it('twenty-one is hyphenated', () => {
    expect(toWords(21)).toBe('twenty-one');
  });

  it('seventy-seven is hyphenated', () => {
    expect(toWords(77)).toBe('seventy-seven');
  });

  it('ninety-nine is hyphenated', () => {
    expect(toWords(99)).toBe('ninety-nine');
  });

  it('one hundred names the leading one', () => {
    expect(toWords(100)).toBe('one hundred');
  });

  it('three hundred three has no "and"', () => {
    expect(toWords(303)).toBe('three hundred three');
  });

  it('five hundred fifty-five keeps the hyphen in the tens', () => {
    expect(toWords(555)).toBe('five hundred fifty-five');
  });

  it('two thousand omits trailing zeros', () => {
    expect(toWords(2000)).toBe('two thousand');
  });

  it('two thousand four hundred skips the tens and ones', () => {
    expect(toWords(2400)).toBe('two thousand four hundred');
  });

  it('three thousand four hundred sixty-six is fully spelled out', () => {
    expect(toWords(3466)).toBe('three thousand four hundred sixty-six');
  });
});
