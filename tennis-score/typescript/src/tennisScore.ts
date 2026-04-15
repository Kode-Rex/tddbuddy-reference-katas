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
  private p1Games = 0;
  private p2Games = 0;
  private p1Sets = 0;
  private p2Sets = 0;
  private gameJustWonBy: 1 | 2 | null = null;
  private setJustWonBy: 1 | 2 | null = null;
  private matchWinner: 1 | 2 | null = null;

  pointWonBy(player: 1 | 2): void {
    if (this.matchWinner !== null) return;

    this.gameJustWonBy = null;
    this.setJustWonBy = null;

    if (player === 1) this.p1Points++;
    else this.p2Points++;

    const [p1, p2] = this.scoreStates();
    if (p1 === 'Game') { this.p1Games++; this.p1Points = 0; this.p2Points = 0; this.gameJustWonBy = 1; }
    else if (p2 === 'Game') { this.p2Games++; this.p1Points = 0; this.p2Points = 0; this.gameJustWonBy = 2; }

    if (this.p1Games >= 6 && this.p1Games - this.p2Games >= 2) {
      this.p1Sets++; this.p1Games = 0; this.p2Games = 0; this.setJustWonBy = 1; this.gameJustWonBy = null;
    } else if (this.p2Games >= 6 && this.p2Games - this.p1Games >= 2) {
      this.p2Sets++; this.p1Games = 0; this.p2Games = 0; this.setJustWonBy = 2; this.gameJustWonBy = null;
    }

    if (this.p1Sets >= 2) { this.matchWinner = 1; this.setJustWonBy = null; }
    else if (this.p2Sets >= 2) { this.matchWinner = 2; this.setJustWonBy = null; }
  }

  score(): string {
    if (this.matchWinner === 1) return 'Match Player 1';
    if (this.matchWinner === 2) return 'Match Player 2';
    if (this.setJustWonBy === 1) return 'Set Player 1';
    if (this.setJustWonBy === 2) return 'Set Player 2';
    if (this.gameJustWonBy === 1) return 'Game Player 1';
    if (this.gameJustWonBy === 2) return 'Game Player 2';

    const [p1, p2] = this.scoreStates();
    if (p1 === 'Advantage') return 'Advantage Player 1';
    if (p2 === 'Advantage') return 'Advantage Player 2';
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
