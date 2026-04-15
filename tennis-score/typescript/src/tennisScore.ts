export class Match {
  private p1Points = 0;
  private p2Points = 0;

  pointWonBy(player: 1 | 2): void {
    if (player === 1) this.p1Points++;
    else this.p2Points++;
  }

  score(): string {
    return `${scoreWord(this.p1Points)}-${scoreWord(this.p2Points)}`;
  }
}

function scoreWord(points: number): string {
  if (points === 0) return 'Love';
  if (points === 1) return '15';
  if (points === 2) return '30';
  return '40';
}
