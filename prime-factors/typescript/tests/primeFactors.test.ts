import { generate } from '../src/primeFactors';

describe('Prime Factors', () => {
  it('1 has no prime factors', () => {
    expect(generate(1)).toEqual([]);
  });

  it('2 is its own only prime factor', () => {
    expect(generate(2)).toEqual([2]);
  });

  it('3 is its own only prime factor', () => {
    expect(generate(3)).toEqual([3]);
  });

  it('4 factors into two twos', () => {
    expect(generate(4)).toEqual([2, 2]);
  });
});
