import { generate } from '../src/primeFactors';

describe('Prime Factors', () => {
  it('1 has no prime factors', () => {
    expect(generate(1)).toEqual([]);
  });
});
