import { describe, it, expect } from 'vitest';
import { greet } from '../src/greeting.js';

describe('greet', () => {
  it('greets a single name by name', () => {
    expect(greet('Bob')).toBe('Hello, Bob.');
  });

  it('greets a null name as "my friend"', () => {
    expect(greet(null)).toBe('Hello, my friend.');
  });

  it('shouts back when the name is all caps', () => {
    expect(greet('JERRY')).toBe('HELLO JERRY!');
  });

  it('joins two names with "and"', () => {
    expect(greet(['Jill', 'Jane'])).toBe('Hello, Jill and Jane');
  });

  it('joins three or more names with an Oxford comma', () => {
    expect(greet(['Amy', 'Brian', 'Charlotte'])).toBe('Hello, Amy, Brian, and Charlotte');
  });

  it('splits mixed normal and shouted names into two greetings', () => {
    expect(greet(['Amy', 'BRIAN', 'Charlotte'])).toBe('Hello, Amy and Charlotte. AND HELLO BRIAN!');
  });
});
