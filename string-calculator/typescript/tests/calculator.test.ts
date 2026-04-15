import { add } from '../src/calculator';

describe('String Calculator', () => {
  it('empty string returns zero', () => {
    expect(add('')).toBe(0);
  });
});
