import { calculateCookies } from '../src/CostCalculator.js';

describe('Cost calculation', () => {
  it('cost is zero when no elves are used', () => {
    expect(calculateCookies(0, 100)).toBe(0);
  });

  it('cost equals elves multiplied by elapsed seconds', () => {
    expect(calculateCookies(5, 10)).toBe(50);
  });

  it('more elves with shorter time can cost the same as fewer elves with longer time', () => {
    const costA = calculateCookies(10, 5);
    const costB = calculateCookies(5, 10);

    expect(costA).toBe(costB);
  });
});
