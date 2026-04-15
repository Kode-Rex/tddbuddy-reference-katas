export function score(rolls: number[]): number {
  return rolls.reduce((total, pins) => total + pins, 0);
}
