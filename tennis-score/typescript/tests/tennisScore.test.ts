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
});
