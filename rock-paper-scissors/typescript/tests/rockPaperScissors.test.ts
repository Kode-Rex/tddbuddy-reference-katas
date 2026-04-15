import { describe, it, expect } from 'vitest';
import { decide } from '../src/rockPaperScissors.js';

describe('decide', () => {
  it('rock vs rock is a draw', () => {
    expect(decide('rock', 'rock')).toBe('draw');
  });

  it('rock vs paper loses because paper covers rock', () => {
    expect(decide('rock', 'paper')).toBe('lose');
  });

  it('rock vs scissors wins because rock crushes scissors', () => {
    expect(decide('rock', 'scissors')).toBe('win');
  });

  it('paper vs rock wins because paper covers rock', () => {
    expect(decide('paper', 'rock')).toBe('win');
  });

  it('paper vs paper is a draw', () => {
    expect(decide('paper', 'paper')).toBe('draw');
  });

  it('paper vs scissors loses because scissors cuts paper', () => {
    expect(decide('paper', 'scissors')).toBe('lose');
  });

  it('scissors vs rock loses because rock crushes scissors', () => {
    expect(decide('scissors', 'rock')).toBe('lose');
  });

  it('scissors vs paper wins because scissors cuts paper', () => {
    expect(decide('scissors', 'paper')).toBe('win');
  });

  it('scissors vs scissors is a draw', () => {
    expect(decide('scissors', 'scissors')).toBe('draw');
  });
});
