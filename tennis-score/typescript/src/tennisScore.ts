export type Score =
  | 'Love'
  | 'Fifteen'
  | 'Thirty'
  | 'Forty'
  | 'Deuce'
  | 'Advantage'
  | 'Game';

export class Match {
  private p1Points = 0;
  private p2Points = 0;

  pointWonBy(player: 1 | 2): void {
    if (player === 1) this.p1Points++;
    else this.p2Points++;
  }

  score(): string {
    const [p1, p2] = this.scoreStates();

    if (p1 === 'Advantage') return 'Advantage Player 1';
    if (p2 === 'Advantage') return 'Advantage Player 2';
    if (p1 === 'Game') return 'Game Player 1';
    if (p2 === 'Game') return 'Game Player 2';
    if (p1 === 'Deuce') return 'Deuce';
    return `${word(p1)}-${word(p2)}`;
  }

  private scoreStates(): [Score, Score] {
    const a = this.p1Points;
    const b = this.p2Points;
    if (a >= 3 && b >= 3) {
      if (a === b) return ['Deuce', 'Deuce'];
      if (a - b === 1) return ['Advantage', 'Forty'];
      if (b - a === 1) return ['Forty', 'Advantage'];
      if (a > b) return ['Game', 'Forty'];
      return ['Forty', 'Game'];
    }
    if (a >= 4) return ['Game', pointsToScore(b)];
    if (b >= 4) return [pointsToScore(a), 'Game'];
    return [pointsToScore(a), pointsToScore(b)];
  }
}

function pointsToScore(points: number): Score {
  if (points === 0) return 'Love';
  if (points === 1) return 'Fifteen';
  if (points === 2) return 'Thirty';
  return 'Forty';
}

function word(s: Score): string {
  switch (s) {
    case 'Love': return 'Love';
    case 'Fifteen': return '15';
    case 'Thirty': return '30';
    case 'Forty': return '40';
    default: return s;
  }
}
