import { describe, it, expect } from 'vitest';
import { justify } from '../src/textJustifier.js';

describe('justify', () => {
  it('returns an empty list for an empty string', () => {
    expect(justify('', 10)).toEqual([]);
  });

  it('returns an empty list for whitespace-only input', () => {
    expect(justify('   \t  ', 10)).toEqual([]);
  });

  it('returns a single word shorter than width as its own last line', () => {
    expect(justify('Word', 10)).toEqual(['Word']);
  });

  it('returns words that fit on one line unjustified', () => {
    expect(justify('Hi there', 20)).toEqual(['Hi there']);
  });

  it('distributes uneven padding left-first across two lines', () => {
    expect(justify('This is a test', 12)).toEqual(['This   is  a', 'test']);
  });

  it('pads each non-last line to width across three lines', () => {
    expect(justify('This is a very long word', 10)).toEqual(['This  is a', 'very  long', 'word']);
  });

  it('collapses multiple consecutive whitespace characters', () => {
    expect(justify('This   is   a   test', 12)).toEqual(['This   is  a', 'test']);
  });

  it('right-pads a single-word non-last line to width', () => {
    expect(justify('longword ab', 9)).toEqual(['longword ', 'ab']);
  });

  it('places a word longer than width on its own line and allows overflow', () => {
    expect(justify('verylongword hi', 5)).toEqual(['verylongword', 'hi']);
  });

  it('distributes evenly when padding divides equally across gaps', () => {
    expect(justify('alpha beta gamma delta epsilon', 25)).toEqual(['alpha  beta  gamma  delta', 'epsilon']);
  });
});
