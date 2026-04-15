export class Match {
  private p1Points = 0;
  private p2Points = 0;

  pointWonBy(player: 1 | 2): void {
    if (player === 1) this.p1Points++;
    else this.p2Points++;
  }

  score(): string {
    if (this.p1Points >= 4 && this.p1Points - this.p2Points === 1) return 'Advantage Player 1';
    if (this.p2Points >= 4 && this.p2Points - this.p1Points === 1) return 'Advantage Player 2';
    if (this.p1Points >= 3 && this.p1Points === this.p2Points) return 'Deuce';
    return `${scoreWord(this.p1Points)}-${scoreWord(this.p2Points)}`;
  }
}

function scoreWord(points: number): string {
  if (points === 0) return 'Love';
  if (points === 1) return '15';
  if (points === 2) return '30';
  return '40';
}
