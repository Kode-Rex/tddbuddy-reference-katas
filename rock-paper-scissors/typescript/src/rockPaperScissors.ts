export type Play = 'rock' | 'paper' | 'scissors';
export type Outcome = 'win' | 'lose' | 'draw';

const beats: Record<Play, Play> = {
  rock: 'scissors',
  scissors: 'paper',
  paper: 'rock',
};

export function decide(p1: Play, p2: Play): Outcome {
  if (p1 === p2) return 'draw';
  return beats[p1] === p2 ? 'win' : 'lose';
}
