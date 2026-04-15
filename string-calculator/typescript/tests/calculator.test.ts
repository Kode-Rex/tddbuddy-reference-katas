import { add } from '../src/calculator';

describe('String Calculator', () => {
  it('empty string returns zero', () => {
    expect(add('')).toBe(0);
  });

  it('single number returns itself', () => {
    expect(add('1')).toBe(1);
  });

  it('two numbers return their sum', () => {
    expect(add('1,2')).toBe(3);
  });

  it('many numbers returns their sum', () => {
    expect(add('1,2,3,4')).toBe(10);
  });

  it('newline is also a delimiter', () => {
    expect(add('1\n2,3')).toBe(6);
  });

  it('custom single-char delimiter is declared in header', () => {
    expect(add('//;\n1;2')).toBe(3);
  });

  it('negative number is rejected with listing message', () => {
    expect(() => add('-1,2')).toThrow('negatives not allowed: -1');
  });

  it('multiple negatives are all listed in the message', () => {
    expect(() => add('-1,-2,3')).toThrow('negatives not allowed: -1, -2');
  });

  it('numbers greater than 1000 are ignored', () => {
    expect(add('2,1001')).toBe(2);
  });

  it('delimiter may be any length in bracketed header', () => {
    expect(add('//[***]\n1***2***3')).toBe(6);
  });
});
