import { Match } from '../src/tennisScore';

describe('Tennis Score', () => {
  it('start of match reads Love-Love', () => {
    expect(new Match().score()).toBe('Love-Love');
  });
});
