import { describe, it, expect } from 'vitest';
import { wrap } from '../src/wordWrap.js';

describe('wrap', () => {
  it('returns an empty string for an empty string', () => {
    expect(wrap('', 10)).toBe('');
  });

  it('returns an empty string for whitespace-only input', () => {
    expect(wrap('   \t  ', 10)).toBe('');
  });

  it('returns a single word shorter than width unchanged', () => {
    expect(wrap('Hello', 10)).toBe('Hello');
  });

  it('returns a single word equal to width unchanged', () => {
    expect(wrap('Hello', 5)).toBe('Hello');
  });

  it('returns two words that fit on one line unchanged', () => {
    expect(wrap('Hello World', 20)).toBe('Hello World');
  });

  it('breaks two words at the word boundary when they do not fit', () => {
    expect(wrap('Hello World', 5)).toBe('Hello\nWorld');
  });

  it('breaks two words at the word boundary when the gap pushes past width', () => {
    expect(wrap('Hello World', 7)).toBe('Hello\nWorld');
  });

  it('places three words across three lines when each line fits one word', () => {
    expect(wrap('Hello wonderful World', 9)).toBe('Hello\nwonderful\nWorld');
  });

  it('splits an oversize single word hard at width', () => {
    expect(wrap('Supercalifragilisticexpialidocious', 10)).toBe(
      'Supercalif\nragilistic\nexpialidoc\nious',
    );
  });

  it('joins an oversize word remainder with the next word when it fits', () => {
    expect(wrap('abcdefghij kl', 5)).toBe('abcde\nfghij\nkl');
  });

  it('collapses multiple consecutive whitespace characters', () => {
    expect(wrap('Hello   World', 5)).toBe('Hello\nWorld');
  });

  it('packs multiple words greedily across multiple lines', () => {
    expect(wrap('The quick brown fox jumps over the lazy dog', 10)).toBe(
      'The quick\nbrown fox\njumps over\nthe lazy\ndog',
    );
  });
});
