export function score(rolls: number[]): number {
  let total = 0;
  let i = 0;
  while (i < rolls.length - 1) {
    if (rolls[i]! + rolls[i + 1]! === 10) {
      total += 10 + rolls[i + 2]!;
    } else {
      total += rolls[i]! + rolls[i + 1]!;
    }
    i += 2;
  }
  return total;
}
