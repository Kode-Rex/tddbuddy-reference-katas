import { describe, it, expect } from 'vitest';
import { getScore } from '../src/tennisScorer.js';

describe('getScore', () => {
  it('Love-All at the start of a game', () => {
    expect(getScore(0, 0, 'Player1', 'Player2')).toBe('Love-All');
  });

  it('Fifteen-All when both players have one point', () => {
    expect(getScore(1, 1, 'Player1', 'Player2')).toBe('Fifteen-All');
  });

  it('Thirty-All when both players have two points', () => {
    expect(getScore(2, 2, 'Player1', 'Player2')).toBe('Thirty-All');
  });

  it('Deuce when both players have three points', () => {
    expect(getScore(3, 3, 'Player1', 'Player2')).toBe('Deuce');
  });

  it('Deuce persists past Forty-All', () => {
    expect(getScore(4, 4, 'Player1', 'Player2')).toBe('Deuce');
  });

  it('Fifteen-Love when player 1 leads by one at the start', () => {
    expect(getScore(1, 0, 'Player1', 'Player2')).toBe('Fifteen-Love');
  });

  it('Love-Fifteen when player 2 leads by one at the start', () => {
    expect(getScore(0, 1, 'Player1', 'Player2')).toBe('Love-Fifteen');
  });

  it('Thirty-Fifteen when player 1 has two and player 2 has one', () => {
    expect(getScore(2, 1, 'Player1', 'Player2')).toBe('Thirty-Fifteen');
  });

  it('Forty-Fifteen when player 1 has three and player 2 has one', () => {
    expect(getScore(3, 1, 'Player1', 'Player2')).toBe('Forty-Fifteen');
  });

  it('Advantage to player 1 when they lead by one in the endgame', () => {
    expect(getScore(4, 3, 'Player1', 'Player2')).toBe('Advantage Player1');
  });

  it('Advantage to player 2 when they lead by one in the endgame', () => {
    expect(getScore(3, 4, 'Player1', 'Player2')).toBe('Advantage Player2');
  });

  it('Advantage persists at higher equal-gap scores', () => {
    expect(getScore(5, 4, 'Player1', 'Player2')).toBe('Advantage Player1');
  });

  it('Win for player 1 when they lead by two in the endgame', () => {
    expect(getScore(5, 3, 'Player1', 'Player2')).toBe('Win for Player1');
  });

  it('Win for player 2 when they lead by two in the endgame', () => {
    expect(getScore(3, 5, 'Player1', 'Player2')).toBe('Win for Player2');
  });

  it('player names are passed through verbatim into Advantage', () => {
    expect(getScore(4, 3, 'Serena', 'Venus')).toBe('Advantage Serena');
  });

  it('player names are passed through verbatim into Win', () => {
    expect(getScore(5, 3, 'Serena', 'Venus')).toBe('Win for Serena');
  });
});
