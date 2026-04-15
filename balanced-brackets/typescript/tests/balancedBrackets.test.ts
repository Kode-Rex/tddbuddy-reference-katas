import { describe, it, expect } from 'vitest';
import { isBalanced } from '../src/balancedBrackets.js';

describe('isBalanced', () => {
  it('empty string is balanced', () => {
    expect(isBalanced('')).toBe(true);
  });

  it('single pair is balanced', () => {
    expect(isBalanced('[]')).toBe(true);
  });

  it('two sequential pairs are balanced', () => {
    expect(isBalanced('[][]')).toBe(true);
  });

  it('nested pair is balanced', () => {
    expect(isBalanced('[[]]')).toBe(true);
  });

  it('deeply nested mixed pairs are balanced', () => {
    expect(isBalanced('[[[][]]]')).toBe(true);
  });

  it('closing before opening is not balanced', () => {
    expect(isBalanced('][')).toBe(false);
  });

  it('alternating reversed pairs are not balanced', () => {
    expect(isBalanced('][][')).toBe(false);
  });

  it('trailing imbalance is not balanced', () => {
    expect(isBalanced('[][]][')).toBe(false);
  });

  it('a lone opener is not balanced', () => {
    expect(isBalanced('[')).toBe(false);
  });

  it('a lone closer is not balanced', () => {
    expect(isBalanced(']')).toBe(false);
  });

  it('unmatched opener at end is not balanced', () => {
    expect(isBalanced('[[]')).toBe(false);
  });

  it('unmatched closer at end is not balanced', () => {
    expect(isBalanced('[]]')).toBe(false);
  });
});
