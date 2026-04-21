import { describe, expect, it } from 'vitest';
import { assertEqual, assertTrue, assertThrows } from '../src/Assertions.js';
import { AssertionFailedException } from '../src/AssertionFailedException.js';

describe('Assertions', () => {
  // --- assertEqual ---

  it('assertEqual with equal values passes', () => {
    expect(() => assertEqual(5, 5)).not.toThrow();
  });

  it('assertEqual with different values fails', () => {
    expect(() => assertEqual(5, 3)).toThrow(AssertionFailedException);
    expect(() => assertEqual(5, 3)).toThrow('expected 5 but got 3');
  });

  // --- assertTrue ---

  it('assertTrue with true passes', () => {
    expect(() => assertTrue(true)).not.toThrow();
  });

  it('assertTrue with false fails', () => {
    expect(() => assertTrue(false)).toThrow(AssertionFailedException);
    expect(() => assertTrue(false)).toThrow('expected true');
  });

  // --- assertThrows ---

  it('assertThrows with throwing function passes', () => {
    expect(() =>
      assertThrows(() => {
        throw new Error('boom');
      })
    ).not.toThrow();
  });

  it('assertThrows with non-throwing function fails', () => {
    expect(() => assertThrows(() => {})).toThrow(AssertionFailedException);
    expect(() => assertThrows(() => {})).toThrow('expected exception');
  });
});
