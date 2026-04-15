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

  it('4-3 reads Advantage Player 1', () => {
    const match = new Match();
    for (let i = 0; i < 3; i++) {
      match.pointWonBy(1);
      match.pointWonBy(2);
    }
    match.pointWonBy(1);
    expect(match.score()).toBe('Advantage Player 1');
  });

  it('4-2 reads Game Player 1', () => {
    const match = new Match();
    match.pointWonBy(1);
    match.pointWonBy(1);
    match.pointWonBy(2);
    match.pointWonBy(1);
    match.pointWonBy(2);
    match.pointWonBy(1);
    expect(match.score()).toBe('Game Player 1');
  });

  it('6-4 in games reads Set Player 1', () => {
    const match = new Match();
    for (let i = 0; i < 4; i++) { playGame(match, 1); playGame(match, 2); }
    playGame(match, 1);
    playGame(match, 1);
    expect(match.score()).toBe('Set Player 1');
  });

  it('6-4, 6-3 in sets reads Match Player 1', () => {
    const match = new Match();
    // Set 1: 6-4
    for (let i = 0; i < 4; i++) { playGame(match, 1); playGame(match, 2); }
    playGame(match, 1);
    playGame(match, 1);
    // Set 2: 6-3
    for (let i = 0; i < 3; i++) { playGame(match, 1); playGame(match, 2); }
    playGame(match, 1);
    playGame(match, 1);
    playGame(match, 1);
    expect(match.score()).toBe('Match Player 1');
  });
});

function playGame(match: Match, winner: 1 | 2): void {
  for (let p = 0; p < 4; p++) match.pointWonBy(winner);
}
