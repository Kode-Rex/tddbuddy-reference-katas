import { describe, it, expect } from 'vitest';
import { trim } from '../src/endOfLineTrim.js';

describe('trim', () => {
  it('returns input unchanged when there is no trailing whitespace', () => {
    expect(trim('abc')).toBe('abc');
  });

  it('removes a trailing space', () => {
    expect(trim('abc ')).toBe('abc');
  });

  it('removes a trailing tab', () => {
    expect(trim('abc\t')).toBe('abc');
  });

  it('preserves leading whitespace', () => {
    expect(trim(' abc')).toBe(' abc');
  });

  it('removes trailing whitespace on each CRLF line', () => {
    expect(trim('ab\r\n cd \r\n')).toBe('ab\r\n cd\r\n');
  });

  it('returns a lone CRLF unchanged', () => {
    expect(trim('\r\n')).toBe('\r\n');
  });

  it('removes trailing whitespace on each LF line', () => {
    expect(trim('ab\n cd \n')).toBe('ab\n cd\n');
  });

  it('collapses a whitespace-only line but keeps its terminator', () => {
    expect(trim('  \n')).toBe('\n');
  });

  it('returns an empty string for empty input', () => {
    expect(trim('')).toBe('');
  });

  it('preserves mixed line endings per line', () => {
    expect(trim('ab \r\ncd \nef ')).toBe('ab\r\ncd\nef');
  });
});
