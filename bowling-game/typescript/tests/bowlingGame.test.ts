import { score } from '../src/bowlingGame';

describe('Bowling Game', () => {
  it('gutter game scores zero', () => {
    expect(score(new Array(20).fill(0))).toBe(0);
  });

  it('all ones scores twenty', () => {
    expect(score(new Array(20).fill(1))).toBe(20);
  });
});
