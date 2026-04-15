import { add } from '../src/calculator';

describe('String Calculator', () => {
  it('empty string returns zero', () => {
    expect(add('')).toBe(0);
  });

  it('single number returns itself', () => {
    expect(add('1')).toBe(1);
  });
});
