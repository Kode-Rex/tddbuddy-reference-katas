import { describe, it, expect } from 'vitest';
import { print } from '../src/diamond.js';

describe('print', () => {
  it('A is a single-letter diamond', () => {
    expect(print('A')).toBe('A');
  });

  it('B renders three rows with a single inner space', () => {
    expect(print('B')).toBe(' A\nB B\n A');
  });

  it('C renders five rows and three inner spaces on the widest row', () => {
    expect(print('C')).toBe('  A\n B B\nC   C\n B B\n  A');
  });

  it('D renders seven rows with a five-space widest row', () => {
    const expected = [
      '   A',
      '  B B',
      ' C   C',
      'D     D',
      ' C   C',
      '  B B',
      '   A',
    ].join('\n');
    expect(print('D')).toBe(expected);
  });

  it('E renders nine rows with a seven-space widest row', () => {
    const expected = [
      '    A',
      '   B B',
      '  C   C',
      ' D     D',
      'E       E',
      ' D     D',
      '  C   C',
      '   B B',
      '    A',
    ].join('\n');
    expect(print('E')).toBe(expected);
  });

  it('Z renders a full 51-row diamond', () => {
    const rows = print('Z').split('\n');
    expect(rows).toHaveLength(51);
    expect(rows[0]).toBe(' '.repeat(25) + 'A');
    expect(rows[25]).toBe('Z' + ' '.repeat(49) + 'Z');
    expect(rows[50]).toBe(' '.repeat(25) + 'A');
  });

  it('lowercase input is normalized to uppercase', () => {
    expect(print('c')).toBe(print('C'));
  });

  it('top half mirrors bottom half', () => {
    const rows = print('F').split('\n');
    const last = rows.length - 1;
    for (let r = 0; r <= last / 2; r++) {
      expect(rows[r]).toBe(rows[last - r]);
    }
  });

  it('no row has trailing whitespace', () => {
    for (const row of print('G').split('\n')) {
      expect(row).toBe(row.replace(/\s+$/, ''));
    }
  });

  it('non-letter input is rejected', () => {
    expect(() => print('1')).toThrow('letter must be a single A-Z character');
  });
});
