export class Match {
  private p1Points = 0;

  pointWonBy(player: 1 | 2): void {
    if (player === 1) this.p1Points++;
  }

  score(): string {
    if (this.p1Points === 1) return '15-Love';
    return 'Love-Love';
  }
}
