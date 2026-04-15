import { describe, it, expect } from 'vitest';
import { Peg, createFeedback } from '../src/codeBreaker.js';
import { SecretBuilder } from './secretBuilder.js';
import { GuessBuilder } from './guessBuilder.js';

const { One, Two, Three, Four, Five, Six } = Peg;

describe('Feedback', () => {
  it('secret 1234 vs guess with no shared pegs has no matches', () => {
    const secret = new SecretBuilder().with(One, Two, Three, Four).build();
    const guess = new GuessBuilder().with(Five, Six, Five, Six).build();

    const feedback = secret.scoreAgainst(guess);

    expect(feedback.render()).toBe('');
    expect(feedback.exactMatches).toBe(0);
    expect(feedback.colorMatches).toBe(0);
  });

  it('secret 1234 vs guess 1566 has one exact match', () => {
    const secret = new SecretBuilder().with(One, Two, Three, Four).build();
    const guess = new GuessBuilder().with(One, Five, Six, Six).build();

    const feedback = secret.scoreAgainst(guess);

    expect(feedback.render()).toBe('+');
    expect(feedback.exactMatches).toBe(1);
    expect(feedback.colorMatches).toBe(0);
  });

  it('secret 1234 vs guess 1234 is a win', () => {
    const secret = new SecretBuilder().with(One, Two, Three, Four).build();
    const guess = new GuessBuilder().with(One, Two, Three, Four).build();

    const feedback = secret.scoreAgainst(guess);

    expect(feedback.render()).toBe('++++');
    expect(feedback.exactMatches).toBe(4);
    expect(feedback.isWon).toBe(true);
  });

  it('secret 1234 vs guess 4321 has four color matches', () => {
    const secret = new SecretBuilder().with(One, Two, Three, Four).build();
    const guess = new GuessBuilder().with(Four, Three, Two, One).build();

    const feedback = secret.scoreAgainst(guess);

    expect(feedback.render()).toBe('----');
    expect(feedback.exactMatches).toBe(0);
    expect(feedback.colorMatches).toBe(4);
  });

  it('secret 1234 vs guess 1243 has two exact and two color matches', () => {
    const secret = new SecretBuilder().with(One, Two, Three, Four).build();
    const guess = new GuessBuilder().with(One, Two, Four, Three).build();

    const feedback = secret.scoreAgainst(guess);

    expect(feedback.render()).toBe('++--');
    expect(feedback.exactMatches).toBe(2);
    expect(feedback.colorMatches).toBe(2);
  });

  it('secret 1234 vs guess 2135 has one exact and two color matches', () => {
    const secret = new SecretBuilder().with(One, Two, Three, Four).build();
    const guess = new GuessBuilder().with(Two, One, Three, Five).build();

    const feedback = secret.scoreAgainst(guess);

    expect(feedback.render()).toBe('+--');
    expect(feedback.exactMatches).toBe(1);
    expect(feedback.colorMatches).toBe(2);
  });

  it('secret 1124 vs guess 5166 counts the duplicate peg only once', () => {
    const secret = new SecretBuilder().with(One, One, Two, Four).build();
    const guess = new GuessBuilder().with(Five, One, Six, Six).build();

    const feedback = secret.scoreAgainst(guess);

    expect(feedback.render()).toBe('+');
    expect(feedback.exactMatches).toBe(1);
    expect(feedback.colorMatches).toBe(0);
  });

  it('secret 1122 vs guess 2211 has four color matches, no exact', () => {
    const secret = new SecretBuilder().with(One, One, Two, Two).build();
    const guess = new GuessBuilder().with(Two, Two, One, One).build();

    const feedback = secret.scoreAgainst(guess);

    expect(feedback.render()).toBe('----');
    expect(feedback.exactMatches).toBe(0);
    expect(feedback.colorMatches).toBe(4);
  });

  it('secret 1111 vs guess 1112 counts three exacts and ignores the non-matching peg', () => {
    const secret = new SecretBuilder().with(One, One, One, One).build();
    const guess = new GuessBuilder().with(One, One, One, Two).build();

    const feedback = secret.scoreAgainst(guess);

    expect(feedback.render()).toBe('+++');
    expect(feedback.exactMatches).toBe(3);
    expect(feedback.colorMatches).toBe(0);
  });

  it('secret 1111 vs guess 2111 counts three exacts at positions 2 through 4', () => {
    const secret = new SecretBuilder().with(One, One, One, One).build();
    const guess = new GuessBuilder().with(Two, One, One, One).build();

    const feedback = secret.scoreAgainst(guess);

    expect(feedback.render()).toBe('+++');
    expect(feedback.exactMatches).toBe(3);
    expect(feedback.colorMatches).toBe(0);
  });

  it('secret 1234 vs guess 1111 counts one exact and no color matches', () => {
    const secret = new SecretBuilder().with(One, Two, Three, Four).build();
    const guess = new GuessBuilder().with(One, One, One, One).build();

    const feedback = secret.scoreAgainst(guess);

    expect(feedback.render()).toBe('+');
    expect(feedback.exactMatches).toBe(1);
    expect(feedback.colorMatches).toBe(0);
  });

  it('a Feedback with four exact matches reports isWon true; any other Feedback reports isWon false', () => {
    expect(createFeedback(4, 0).isWon).toBe(true);
    expect(createFeedback(0, 4).isWon).toBe(false);
    expect(createFeedback(3, 1).isWon).toBe(false);
    expect(createFeedback(0, 0).isWon).toBe(false);
  });
});
