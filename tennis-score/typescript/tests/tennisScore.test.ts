import { Match } from '../src/tennisScore';

describe('Tennis Score', () => {
  it('start of match reads Love-Love', () => {
    expect(new Match().score()).toBe('Love-Love');
  });

  it('one point to player 1 reads 15-Love', () => {
    const match = new Match();
    match.pointWonBy(1);
    expect(match.score()).toBe('15-Love');
  });

  it('two points each reads 30-30', () => {
    const match = new Match();
    match.pointWonBy(1);
    match.pointWonBy(2);
    match.pointWonBy(1);
    match.pointWonBy(2);
    expect(match.score()).toBe('30-30');
  });

  it('three points each reads Deuce', () => {
    const match = new Match();
    for (let i = 0; i < 3; i++) {
      match.pointWonBy(1);
      match.pointWonBy(2);
    }
    expect(match.score()).toBe('Deuce');
  });
});
