import { score } from '../src/bowlingGame';

describe('Bowling Game', () => {
  it('gutter game scores zero', () => {
    expect(score(new Array(20).fill(0))).toBe(0);
  });

  it('all ones scores twenty', () => {
    expect(score(new Array(20).fill(1))).toBe(20);
  });

  it('one spare scores the spare bonus', () => {
    const rolls = [5, 5, 3, 0, ...new Array(16).fill(0)];
    expect(score(rolls)).toBe(16);
  });

  it('one strike scores the strike bonus', () => {
    const rolls = [10, 3, 4, ...new Array(16).fill(0)];
    expect(score(rolls)).toBe(24);
  });

  it('perfect game scores 300', () => {
    expect(score(new Array(12).fill(10))).toBe(300);
  });

  it('all spares scores 150', () => {
    expect(score(new Array(21).fill(5))).toBe(150);
  });
});
