import { describe, it, expect } from 'vitest';
import { say } from '../src/fizzBuzzWhiz.js';

describe('FizzBuzzWhiz.say', () => {
  it('returns "1" for 1', () => {
    expect(say(1)).toBe('1');
  });

  it('returns "2" for 2', () => {
    expect(say(2)).toBe('2');
  });

  it('returns "Fizz" for 3 (divisible by three)', () => {
    expect(say(3)).toBe('Fizz');
  });

  it('returns "Buzz" for 5 (divisible by five)', () => {
    expect(say(5)).toBe('Buzz');
  });

  it('returns "Fizz" for 6 (another multiple of three)', () => {
    expect(say(6)).toBe('Fizz');
  });

  it('returns "Buzz" for 10 (another multiple of five)', () => {
    expect(say(10)).toBe('Buzz');
  });

  it('returns "FizzBuzz" for 15 (divisible by both three and five)', () => {
    expect(say(15)).toBe('FizzBuzz');
  });

  it('returns "FizzBuzz" for 30 (another multiple of fifteen)', () => {
    expect(say(30)).toBe('FizzBuzz');
  });
});
