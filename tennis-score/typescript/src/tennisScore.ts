export class Match {
  private p1Points = 0;
  private p2Points = 0;

  pointWonBy(player: 1 | 2): void {
    if (player === 1) this.p1Points++;
    else this.p2Points++;
  }

  score(): string {
    let p1Word: string;
    if (this.p1Points === 0) p1Word = 'Love';
    else if (this.p1Points === 1) p1Word = '15';
    else if (this.p1Points === 2) p1Word = '30';
    else p1Word = '40';

    let p2Word: string;
    if (this.p2Points === 0) p2Word = 'Love';
    else if (this.p2Points === 1) p2Word = '15';
    else if (this.p2Points === 2) p2Word = '30';
    else p2Word = '40';

    return `${p1Word}-${p2Word}`;
  }
}
