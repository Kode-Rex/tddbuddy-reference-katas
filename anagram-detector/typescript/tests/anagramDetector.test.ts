import { describe, it, expect } from 'vitest';
import { areAnagrams, findAnagrams, groupAnagrams } from '../src/anagramDetector.js';

describe('areAnagrams', () => {
  it('listen and silent are anagrams', () => {
    expect(areAnagrams('listen', 'silent')).toBe(true);
  });

  it('hello and world are not anagrams', () => {
    expect(areAnagrams('hello', 'world')).toBe(false);
  });

  it('cat and tac are anagrams', () => {
    expect(areAnagrams('cat', 'tac')).toBe(true);
  });

  it('a word is not an anagram of itself', () => {
    expect(areAnagrams('cat', 'cat')).toBe(false);
  });

  it('comparison is case insensitive', () => {
    expect(areAnagrams('Cat', 'tac')).toBe(true);
  });

  it('empty strings are not anagrams', () => {
    expect(areAnagrams('', '')).toBe(false);
  });

  it('single identical letters are not anagrams', () => {
    expect(areAnagrams('a', 'a')).toBe(false);
  });

  it('ab and ba are anagrams', () => {
    expect(areAnagrams('ab', 'ba')).toBe(true);
  });

  it('aab and abb are not anagrams because letter counts differ', () => {
    expect(areAnagrams('aab', 'abb')).toBe(false);
  });

  it('Astronomer and Moon starer are anagrams ignoring case and whitespace', () => {
    expect(areAnagrams('Astronomer', 'Moon starer')).toBe(true);
  });

  it('rail safety and fairy tales are anagrams across multi-word phrases', () => {
    expect(areAnagrams('rail safety', 'fairy tales')).toBe(true);
  });
});

describe('findAnagrams', () => {
  it('returns all matching candidates', () => {
    expect(findAnagrams('listen', ['silent', 'tinsel'])).toEqual(['silent', 'tinsel']);
  });

  it('returns empty when no candidate matches', () => {
    expect(findAnagrams('listen', ['hello', 'world'])).toEqual([]);
  });

  it('preserves input order for mixed matches', () => {
    expect(findAnagrams('master', ['stream', 'maters', 'pigeon'])).toEqual(['stream', 'maters']);
  });
});

describe('groupAnagrams', () => {
  it('collects words sharing one anagram key', () => {
    expect(groupAnagrams(['eat', 'tea', 'ate'])).toEqual([['eat', 'tea', 'ate']]);
  });

  it('returns singletons when no keys are shared', () => {
    expect(groupAnagrams(['abc', 'def'])).toEqual([['abc'], ['def']]);
  });

  it('returns an empty list for no words', () => {
    expect(groupAnagrams([])).toEqual([]);
  });

  it('preserves first-occurrence order of groups and members', () => {
    expect(groupAnagrams(['eat', 'tea', 'tan', 'ate', 'nat', 'bat'])).toEqual([
      ['eat', 'tea', 'ate'],
      ['tan', 'nat'],
      ['bat'],
    ]);
  });
});
