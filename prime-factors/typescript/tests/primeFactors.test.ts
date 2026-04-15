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

  it('6 factors into two and three', () => {
    expect(generate(6)).toEqual([2, 3]);
  });

  it('8 factors into three twos', () => {
    expect(generate(8)).toEqual([2, 2, 2]);
  });

  it('9 factors into two threes', () => {
    expect(generate(9)).toEqual([3, 3]);
  });

  it('12 factors into two, two, three', () => {
    expect(generate(12)).toEqual([2, 2, 3]);
  });

  it('15 factors into three and five', () => {
    expect(generate(15)).toEqual([3, 5]);
  });

  it('100 factors into two, two, five, five', () => {
    expect(generate(100)).toEqual([2, 2, 5, 5]);
  });
});
